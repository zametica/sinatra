using Microsoft.AspNetCore.Mvc;
using Sinatra.Api.Models.Auth;
using Sinatra.WebApi.Services;

namespace Sinatra.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }


    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest body)
    {
        return Ok(await _authService.LoginAsync(body));
    }

    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync(object body)
    {
        throw new NotImplementedException();
    }

    [HttpPost("refreshtoken")]
    public async Task<ActionResult> RefreshTokenAsync(RefreshTokenRequest body)
    {
        return Ok(await _authService.RefreshTokenAsync(body));
    }

    [HttpPost("changepassword")]
    public async Task<ActionResult> ChangePasswordAsync(object body)
    {
        throw new NotImplementedException();
    }

    [HttpPost("resetpassword")]
    public async Task<ActionResult> ResetPasswordAsync(object body)
    {
        throw new NotImplementedException();
    }

    [HttpPost("resetpassword/{token}")]
    public async Task<ActionResult> ResetPasswordConfirmAsync(object body, string token)
    {
        throw new NotImplementedException();
    }

    [HttpPost("verifyemail")]
    public async Task<ActionResult> VerifyEmailAsync(object body)
    {
        throw new NotImplementedException();
    }

    [HttpPost("verifyemail/{token}")]
    public async Task<ActionResult> VerifyEmailConfirmAsync(object body, string token)
    {
        throw new NotImplementedException();
    }
}
