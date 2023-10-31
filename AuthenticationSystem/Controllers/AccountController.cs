using AuthenticationSystem.Logic.Converters;
using AuthenticationSystem.Logic.Users;
using AuthenticationSystem.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationSystem.Domain.Core;

namespace UserAuthenticationSystem.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IUserManager<User, CreateUserRequest> _manager;

        public AccountController(ILogger<AccountController> logger,
            IUserManager<User, CreateUserRequest> manager)
        {
            _manager = manager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> LoginAsync([FromBody] LoginRequest request)
        {
            var user = await _manager.LoginAsync(request.Email, request.Password);
            if (user.IsFailed)
            {
                return BadRequest(user.Errors.First().Message);
            }

            return Ok(UserConverter.Convert(user.Value));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> RegisterAsync([FromBody] CreateUserRequest request)
        {
            var user = await _manager.CreateUser(request);
            if (user.IsFailed)
            {
                return BadRequest(user.Errors.First().Message);
            }

            return Ok(UserConverter.Convert(user.Value));
        }
    }
}