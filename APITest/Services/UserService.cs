using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APITest.Authorization;
using APITest.Exceptions.BadRequest;
using APITest.Exceptions.Conflict;
using APITest.Exceptions.NotFound;
using APITest.Models;
using APITest.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DTO;


namespace APITest.Services
{
    public class UserService : IUserService
    {
        private TodoContext _context;
        private IConfiguration _config;
        private IPersonService _personService;

        public UserService(TodoContext context, IPersonService personService, IConfiguration config)
        {
            _context = context;
            _config = config;
            _personService = personService;
        }

        /// <summary>
        /// Returns a JSON web token if successful, returns null otherwise
        /// </summary>
        /// <param name="submittedUser"></param>
        /// <returns></returns>
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

            if (string.IsNullOrEmpty(submittedUser.Role))
            {
                user.Role = await GetUserRoleId("user");
            }
            else
            {
                user.Role = await GetUserRoleId(submittedUser.Role);
            }

            if (await UniquePropertyExists(user))
            {
                throw new BadRequestException("User", user.Username);
            }
            // Add a person entity to the database and assign that person to the user being added
            user.Person = await _personService.Create(submittedUser.Person);

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


        public async Task<string> Update(string id, UserPut submittedUser)
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
                await _personService.Update(submittedUser.UserId, submittedUser.Person);
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

            return await Login(new UserCredentials() { Username = user.Username, Password = submittedUser.Password });
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
        /// Returns a token if the user credentials match, returns null if they do not
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<string> Login(UserCredentials credentials)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == credentials.Username);

            if (user == null)
            {
                throw new UserNotFoundException(user.Username);
            }

            if (Auth.VerifyPassword(credentials.Password, user.Salt, user.HashedPassword))
            {
                return await GenerateToken(user);
            }

            return null;
        }

        public async Task<string> GenerateToken(User user)
        {
            return await Task.Run(() =>
            {
                user = _context.Entry(user).Entity;

                List<Claim> claims = new List<Claim>()
                {
                    // Create a new guid so the JWT id can only be used once
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                    // set JWT issued at time
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    // Set the JWT user id as the subscriber
                    new Claim(CustomClaims.UserId, user.Id),
                    // Add the user role to the claims
                    new Claim(CustomClaims.UserRole, user.Role.UserRole),
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config.GetSection("jwt").GetSection("secret").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(issuer: _config.GetSection("jwt").GetSection("issuer").Value,
                    audience: _config.GetSection("jwt").GetSection("audience").Value,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: credentials
                );

                var tokenHandler = new JwtSecurityTokenHandler();

                return tokenHandler.WriteToken(token);
            });
        }

        private async Task<bool> UserExists(string id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> UniquePropertyExists(User user)
        {
            return await _context.Persons.AnyAsync(p => user.Person.Email == p.Email);
        }

        private async Task<Role> GetUserRoleId(string role)
        {
            var roleById = await _context.Roles.FirstOrDefaultAsync(r => r.UserRole.ToLower() == role.ToLower());

            if (roleById == null)
            {
                // Default to user role
                roleById = await _context.Roles.FirstOrDefaultAsync(r => r.UserRole.ToLower() == "user");
            }

            return roleById;
        }
    }
}
