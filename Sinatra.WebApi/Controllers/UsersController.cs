using Microsoft.AspNetCore.Mvc;
using Sinatra.Api.Models.Users;
using Sinatra.WebApi.Helpers.Authorization;
using Sinatra.WebApi.Services;
using Role = Sinatra.WebApi.Data.Models.Role;

namespace Sinatra.WebApi.Controllers;

[ApiController]
[Route("api/users")]
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
        return StatusCode(201, await _userService.CreateUserAsync(body));
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Role.USER)]
    public async Task<ActionResult<GetUserResponse>> GetUserAsync(Guid userId)
    {
        return Ok(await _userService.GetUserAsync(userId));
    }
}
