using Microsoft.AspNetCore.Mvc;
using Sinatra.Api.Models.Users;
using Sinatra.WebApi.Services;

namespace Sinatra.WebApi.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest body)
        {
            return StatusCode(200, await _userService.CreateUserAsync(body));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserResponse>> GetUserAsync(Guid userId)
        {
            return Ok(await _userService.GetUserAsync(userId));
        }
    }
}
