using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APITest.Models;
using APITest.Shared;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TodoContext _context;

        public UsersController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserPut submittedUser)
        {
            string userId;
            // Attempt to get the userId cookie out of the request headers
            if (Request.Headers.TryGetValue("UserId", out StringValues cookieId))
            {
                userId = cookieId.ToString();
            }
            else
            {
                return BadRequest();
            }

            if (id != submittedUser.UserId || id != userId)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid || id != submittedUser.Person.Id)
            {
                return BadRequest();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == submittedUser.UserId);

            if (!string.IsNullOrEmpty(submittedUser.Password))
            {
                var saltedPassword = Auth.GeneratePassword(submittedUser.Password);
                user.HashedPassword = saltedPassword.HashedPassword;
                user.Salt = saltedPassword.Salt;
            }

            _context.Entry(submittedUser.Person).State = EntityState.Modified;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await UserExists(id) == false)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserPost submittedUser)
        {
            // if not a valid submission, send 400 response
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
                return BadRequest();
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
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<HttpResponseMessage>> Login(UserCredentials userCredentials)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userCredentials.Username);

            if (user == null)
            {
                return NotFound();
            }

            if (Auth.VerifyPassword(userCredentials.Password, user.Salt, user.HashedPassword))
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                var cookie = new CookieHeaderValue("UserId", user.Id);
                cookie.Expires = DateTime.Now.AddMinutes(1);
                response.Headers.AddCookies(new CookieHeaderValue[] {cookie});
                return response;
            }

            return Unauthorized();
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
