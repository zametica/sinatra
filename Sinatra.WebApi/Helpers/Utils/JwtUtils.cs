using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sinatra.WebApi.Data.Models;
using Sinatra.WebApi.Helpers.Authorization;
using Sinatra.WebApi.Helpers.Configuration;

namespace Sinatra.WebApi.Helpers.Utils;

public interface IJwtUtils
{
    public string GenerateJwtToken(Guid userId, Role role);
    public UserProperties ValidateJwtToken(string token);
    public string GenerateRefreshToken();
    public UserProperties GetUserClaimsFromExpiredToken(string token);
    public long GetRefreshTokenValidityInDays();
}

public class JwtUtils : IJwtUtils
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly byte[] _key;
    
    public JwtUtils(IOptions<JwtConfiguration> jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration.Value;
        _key = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
    }
    
    
    public string GenerateJwtToken(Guid userId, Role role)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = new Dictionary<string, object>()
            {
                {"user_id", userId.ToString()},
                {"role", role.ToString()}
            },
            Expires = DateTime.UtcNow.AddSeconds(_jwtConfiguration.TokenValidityInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public UserProperties ValidateJwtToken(string token)
    {
        _tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false, // todo: set to true
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken securityToken);

        return ParseClaims(securityToken);
    }

    public UserProperties GetUserClaimsFromExpiredToken(string token)
    {
        _tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        }, out SecurityToken securityToken);

        return ParseClaims(securityToken);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public long GetRefreshTokenValidityInDays()
    {
        return _jwtConfiguration.RefreshTokenValidityInDays;
    }

    private UserProperties ParseClaims(SecurityToken securityToken)
    {
        JwtSecurityToken jwt = (JwtSecurityToken) securityToken;
        return new UserProperties
        {
            Id = Guid.Parse(jwt.Claims.First(x => x.Type == "user_id").Value),
            Role = (Role) Enum.Parse(typeof(Role), jwt.Claims.First(x => x.Type == "role").Value)
        };
    }
}
