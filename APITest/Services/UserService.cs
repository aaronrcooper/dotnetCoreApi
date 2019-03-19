using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Exceptions.BadRequest;
using APITest.Exceptions.Conflict;
using APITest.Exceptions.NotFound;
using APITest.Models;
using APITest.Shared;
using Microsoft.EntityFrameworkCore;

namespace APITest.Services
{
    public class UserService : IUserService
    {
        public TodoContext _context;
        public UserService(TodoContext context)
        {
            _context = context;
        }

        public async Task<User> Create(UserPost submittedUser)
        {
            SaltedPassword password = Auth.GeneratePassword(submittedUser.Password);
            User user = new User()
            {
                HashedPassword = password.HashedPassword,
                Salt = password.Salt,
                Username = submittedUser.Username,
                Person = submittedUser.Person
            };

            if (await UniquePropertyExists(user))
            {
                throw new BadRequestException("User", user.Username);
            }
            // Add a person entity to the database and assign that person to the user being added
            user.Person = _context.Persons.Add(submittedUser.Person).Entity;

            // Add user to database
            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await UserExists(user.Id))
                {
                    throw new ConflictException("User", user.Id);
                }
                else
                {
                    throw;
                }
            }

            return user;
        }

        public void Delete(string id)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            _context.Remove(user);
        }

        public async Task<User> Get(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> IsAdmin(User User)
        {
            if (User == null)
            {
                return false;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.RoleId == User.RoleId);

            if (user == null)
            {
                return false;
            }

            await _context.Entry(user).Reference(u => u.Role).LoadAsync();
            return user.Role.UserRole.ToUpper() == "ADMININSTATOR";
        }

        /// <summary>
        /// Returns user object if the user credentials match, returns null if they do not
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<User> Login(UserCredentials credentials)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == credentials.Username);

            if (user == null)
            {
                throw new UserNotFoundException(user.Username);
            }

            if (Auth.VerifyPassword(credentials.Password, user.Salt, user.HashedPassword))
            {
                return user;
            }

            return null;
        }


        public async Task<User> Update(string id, UserPut submittedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == submittedUser.UserId);

            if (!string.IsNullOrEmpty(submittedUser.Password))
            {
                var saltedPassword = Auth.GeneratePassword(submittedUser.Password);
                user.HashedPassword = saltedPassword.HashedPassword;
                user.Salt = saltedPassword.Salt;
            }

            if (submittedUser.Person != null)
            {
                _context.Entry(submittedUser.Person).State = EntityState.Modified;
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await UserExists(id) == false)
                {
                    throw new ConflictException("User", id);
                }
                else
                {
                    throw;
                }
            }

            return user;
        }

        private async Task<bool> UserExists(string id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> UniquePropertyExists(User user)
        {
            return await _context.Persons.AnyAsync(p => user.Person.Email == p.Email);
        }
    }
}
