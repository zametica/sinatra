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
        var user = await _db.Users
            .Where(x => x.IdentityType.Equals(IdentityType.EMAIL) && x.Identity.Equals(request.Email))
            .FirstOrDefaultAsync();
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new Exception(); // email or password invalid
        }

        return await GenerateTokenPairAsync<LoginResponse>(user.Id, user.Role);
    }

    public async Task<BindDeviceResponse> BindDeviceAsync(BindDeviceRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x =>
            x.IdentityType == IdentityType.DEVICE && x.Identity.Equals(request.DeviceId));

        if (user == null)
        {
            user = new User()
            {
                IdentityType = IdentityType.DEVICE,
                Identity = request.DeviceId,
                Name = $"Generated{Guid.NewGuid()}",
                Role = Role.TEMP_USER
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        return await GenerateTokenPairAsync<BindDeviceResponse>(user.Id, user.Role);
    }
    
    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userProperties = _jwtUtils.GetUserClaimsFromExpiredToken(request.AccessToken);

        var oldRefreshToken = await _db.RefreshTokens
            .Include(x => x.TokenFamily)
            .FirstOrDefaultAsync(x =>
                x.Token.Equals(request.RefreshToken) &&
                x.TokenFamily.ExpirationTime >= DateTimeOffset.Now &&
                x.TokenFamily.UserId.Equals(userProperties.Id));

        if (oldRefreshToken == null)
        {
            throw new Exception("AccessToken or refreshToken invalid."); // todo: return 401
        }

        if (!oldRefreshToken.Valid)
        {
            _db.TokenFamilies.Remove(oldRefreshToken.TokenFamily);
            await _db.SaveChangesAsync();
            
            throw new Exception("RefreshToken reuse protection"); // todo: return 401
        }

        oldRefreshToken.Valid = false;

        var newRefreshToken = new RefreshToken
        {
            Token = _jwtUtils.GenerateRefreshToken(),
            Valid = true,
            TokenFamilyId = oldRefreshToken.TokenFamilyId
        };
        _db.RefreshTokens.Add(newRefreshToken);
        await _db.SaveChangesAsync();

        var accessToken = _jwtUtils.GenerateJwtToken(userProperties.Id, userProperties.Role);

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }

    public async Task LogoutAsync(LogoutRequest request)
    {
        var refreshToken = await _db.RefreshTokens
            .Include(x => x.TokenFamily)
            .FirstOrDefaultAsync(x => x.Token.Equals(request.RefreshToken));

        if (refreshToken != null)
        {
            _db.TokenFamilies.Remove(refreshToken.TokenFamily);
            await _db.SaveChangesAsync();
        }
    }

    private async Task<T> GenerateTokenPairAsync<T>(Guid userId, Role role) where T : TokenPair, new()
    {
        var refreshToken = new RefreshToken
        {
            Token = _jwtUtils.GenerateRefreshToken(),
            Valid = true
        };
        
        var tokenFamily = new TokenFamily
        {
            UserId = userId,
            ExpirationTime = DateTimeOffset.Now.AddDays(_jwtUtils.GetRefreshTokenValidityInDays()),
            RefreshTokens = new List<RefreshToken> { refreshToken }
        };

        _db.TokenFamilies.Add(tokenFamily);
        await _db.SaveChangesAsync();
        
        var accessToken = _jwtUtils.GenerateJwtToken(userId, role);
        
        return new T()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
    }
}
