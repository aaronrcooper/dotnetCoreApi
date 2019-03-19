﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APITest.Exceptions.BadRequest;
using APITest.Exceptions.Conflict;
using APITest.Exceptions.NotFound;
using APITest.Models;
using APITest.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APITest.Services
{
    public class UserService : IUserService
    {
        private TodoContext _context;
        private IConfiguration _config;

        public UserService(TodoContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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
        public async Task<JwtSecurityToken> Login(UserCredentials credentials)
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

        public async Task<JwtSecurityToken> GenerateToken(User user)
        {
            return await Task.Run(() =>
            {
                List<Claim> claims = new List<Claim>()
                {
                    // Create a new guid so the JWT id can only be used once
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                    // Set the JWT user id as the subscriber
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    // set JWT issued at time
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim("role", user.Role.UserRole),
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config.GetSection("jwt").GetSection("secret").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config.GetSection("jwt").GetSection("issuer").Value,
                    _config.GetSection("jwt").GetSection("audience").Value,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(1),
                    credentials
                );

                return token;
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
    }
}
