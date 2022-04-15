using Sinatra.Api.Models.Auth;

namespace Sinatra.WebApi.Services;

public interface IAuthService
{
    public Task<LoginResponse> LoginAsync(LoginRequest request);
    public Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    public Task LogoutAsync(LogoutRequest request);
    public Task<BindDeviceResponse> BindDeviceAsync(BindDeviceRequest request);
}
