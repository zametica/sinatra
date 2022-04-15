using Microsoft.AspNetCore.Mvc;
using Sinatra.Api.Models.Users;
using Sinatra.WebApi.Helpers.Authorization;
using Sinatra.WebApi.Services;

namespace Sinatra.WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Policy = AuthorizationPolicy.PERMANENT_USER)]
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
    [AllowAnonymous]
    public async Task<ActionResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest body)
    {
        return StatusCode(201, await _userService.CreateUserAsync(body));
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<GetUserResponse>> GetUserAsync(Guid userId)
    {
        return Ok(await _userService.GetUserAsync(userId));
    }
}
