using System.Data;
using Microsoft.EntityFrameworkCore;
using Sinatra.Api.Models.Auth;
using Sinatra.WebApi.Data.Context;
using Sinatra.WebApi.Data.Models;
using Sinatra.WebApi.Helpers.Authorization;
using Sinatra.WebApi.Helpers.Utils;

namespace Sinatra.WebApi.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IJwtUtils _jwtUtils;
    private readonly UserContext _userContext;
    private readonly AppDbContext _db;

    public AuthService(ILogger<AuthService> logger, IJwtUtils jwtUtils, UserContext userContext, AppDbContext db)
    {
        _logger = logger;
        _jwtUtils = jwtUtils;
        _userContext = userContext;
        _db = db;
    }
    
    
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.Where(x => x.Email.Equals(request.Email)).FirstOrDefaultAsync();
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new Exception(); // email or password invalid
        }
        
        var userSession = new UserSession
        {
            UserId = user.Id,
            RefreshToken = _jwtUtils.GenerateRefreshToken(),
            RefreshTokenExpirationTime = DateTimeOffset.Now.AddDays(_jwtUtils.GetRefreshTokenValidityInDays())
        };
        _db.UserSessions.Add(userSession);
        await _db.SaveChangesAsync();
        
        var accessToken = _jwtUtils.GenerateJwtToken(user.Id, user.Role);
        
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = userSession.RefreshToken
        };
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var expiredTokenClaims = _jwtUtils.GetUserClaimsFromExpiredToken(request.AccessToken);
        
        var userSession = await _db.UserSessions
            .FirstOrDefaultAsync(x =>
                x.UserId.Equals(expiredTokenClaims.Id) &&
                x.RefreshToken.Equals(request.RefreshToken) &&
                x.RefreshTokenExpirationTime >= DateTimeOffset.Now);
        
        if (userSession is null)
        {
            throw new Exception(); // accessToken or refreshToken invalid
        }

        var accessToken = _jwtUtils.GenerateJwtToken(expiredTokenClaims.Id, expiredTokenClaims.Role);
        var refreshToken = _jwtUtils.GenerateRefreshToken();

        userSession.RefreshToken = refreshToken;
        await _db.SaveChangesAsync();

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}