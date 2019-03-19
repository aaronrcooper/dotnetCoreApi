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
using APITest.Exceptions.BadRequest;
using APITest.Exceptions.Conflict;
using APITest.Exceptions.NotFound;
using APITest.Services;
using Microsoft.Extensions.Primitives;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService UserService;

        public UsersController(IUserService userService)
        {
            UserService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [Produces(typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await UserService.GetAll());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Produces(typeof(User))]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await UserService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserPut submittedUser)
        {
            try
            {
                await UserService.Update(id, submittedUser);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            catch (ConflictException)
            {
                return Conflict();
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

            User user;
            try
            {
                user = await UserService.Create(submittedUser);
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                UserService.Delete(id);
            }
            catch (UserNotFoundException )
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserCredentials userCredentials)
        {
            try
            {
                var user = await UserService.Login(userCredentials);
                if (user != null)
                {
                    return Ok(user.EncodedPayload);
                }
            }
            catch (UserNotFoundException)
            {
                return BadRequest();
            }

            return Unauthorized();
        }
    }
}
