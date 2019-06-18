using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business.Exceptions.BadRequest;
using Business.Exceptions.Conflict;
using Business.Exceptions.NotFound;
using Business.Services;
using System;
using AutoMapper;
using APITest.Domain.Models;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly IAuthorizationService AuthorizationService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IAuthorizationService authorizationService, IMapper mapper)
        {
            UserService = userService;
            AuthorizationService = authorizationService;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        [Produces(typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetUsers()
        {
            var users = await UserService.GetAll();

            return Ok(_mapper.Map<IEnumerable<User>, IEnumerable<DTO.User>>(users));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Produces(typeof(DTO.User))]
        public async Task<IActionResult> GetUser(System.Guid id)
        {
            var user = await UserService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<User, DTO.User>(user));
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize]
        [Produces(typeof(DTO.LoggedInUser))]
        public async Task<IActionResult> EditUser(Guid id, UserPut submittedUser)
        {
            var authorization = await AuthorizationService.AuthorizeAsync(User, submittedUser.UserId, "IsCurrentUser");

            if (!authorization.Succeeded)
            {
                return Unauthorized();
            }
            try
            {
                var loggedInUser = await UserService.Update(id, submittedUser);
                return Ok(_mapper.Map<LoggedInUser, DTO.LoggedInUser>(loggedInUser));
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
        [Produces(typeof(DTO.User))]
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
                return Ok(_mapper.Map<User, DTO.User>(user));
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
        public IActionResult DeleteUser(Guid id)
        {
            try
            {
                UserService.Delete(id);
                return NoContent();
            }
            catch (UserNotFoundException )
            {
                return BadRequest();
            }

        }

        [HttpPost("Login")]
        [Produces(typeof(DTO.LoggedInUser))]
        public async Task<IActionResult> Login(UserCredentials userCredentials)
        {
            try
            {
                var loggedInUser = await UserService.Login(userCredentials);
                // if token was successfully generated, return OK
                if (!string.IsNullOrEmpty(loggedInUser.JSONWebToken))
                {
                    return Ok(_mapper.Map<LoggedInUser, DTO.LoggedInUser>(loggedInUser));
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
