using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APITest.Models;
using APITest.Exceptions.BadRequest;
using APITest.Exceptions.Conflict;
using APITest.Exceptions.NotFound;
using APITest.Services;
using Microsoft.AspNetCore.Authorization;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly IAuthorizationService AuthorizationService;

        public UsersController(IUserService userService, IAuthorizationService authorizationService)
        {
            UserService = userService;
            AuthorizationService = authorizationService;
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
        [Authorize]
        [Produces(typeof(string))]
        public async Task<IActionResult> EditUser(string id, UserPut submittedUser)
        {
            var authorization = await AuthorizationService.AuthorizeAsync(User, submittedUser.UserId, "IsCurrentUser");

            if (!authorization.Succeeded)
            {
                return Unauthorized();
            }
            try
            {
                var token = await UserService.Update(id, submittedUser);
                return Ok(token);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
        }

        // POST: api/Users
        [HttpPost]
        [Produces(typeof(User))]
        public async Task<IActionResult> CreateUser(UserPost submittedUser)
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
                return Ok(user);
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await Task.Run(() => UserService.Delete(id));
                return NoContent();
            }
            catch (UserNotFoundException )
            {
                return BadRequest();
            }

        }

        [HttpPost("Login")]
        [Produces(typeof(string))]
        public async Task<IActionResult> Login(UserCredentials userCredentials)
        {
            try
            {
                var token = await UserService.Login(userCredentials);
                if (!string.IsNullOrEmpty(token))
                {
                    return Ok(token);
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
