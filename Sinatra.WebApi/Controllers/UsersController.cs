using Microsoft.AspNetCore.Mvc;
using Sinatra.Api.Models.Users;
using Sinatra.WebApi.Helpers.Authorization;
using Sinatra.WebApi.Services;
using Role = Sinatra.WebApi.Data.Models.Role;

namespace Sinatra.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        [HttpPost]
        public async Task<ActionResult<CreateUserResponse>> CreateUserAsync([FromHeader(Name = "Authorization")] string authorizationHeader, CreateUserRequest body)
        {
            return StatusCode(201, await _userService.CreateUserAsync(body));
        }

        [Authorize(Role.USER)]
        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserResponse>> GetUserAsync(Guid userId)
        {
            var response = await _userService.GetUserAsync(userId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost("auth/login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest body)
        {
            return Ok(await _userService.LoginAsync(body));
        }
    }
}
