using Microsoft.AspNetCore.Mvc;
using Sinatra.Api.Models.Auth;
using Sinatra.WebApi.Helpers.Authorization;
using Sinatra.WebApi.Services;

namespace Sinatra.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
[Authorize(Policy = AuthorizationPolicy.PERMANENT_USER)]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest body)
    {
        return Ok(await _authService.LoginAsync(body));
    }

    [AllowAnonymous]
    [HttpPost("bind-device")]
    public async Task<ActionResult<BindDeviceResponse>> BindDeviceAsync(BindDeviceRequest body)
    {
        return Ok(await _authService.BindDeviceAsync(body));
    }
    
    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync(LogoutRequest body)
    {
        await _authService.LogoutAsync(body);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshTokenAsync(RefreshTokenRequest body)
    {
        return Ok(await _authService.RefreshTokenAsync(body));
    }

    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePasswordAsync(object body)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPasswordAsync(object body)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("reset-password/{token}")]
    public async Task<ActionResult> ResetPasswordConfirmAsync(object body, string token)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("verify-email")]
    public async Task<ActionResult> VerifyEmailAsync(object body)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("verify-email/{token}")]
    public async Task<ActionResult> VerifyEmailConfirmAsync(object body, string token)
    {
        throw new NotImplementedException();
    }
}
