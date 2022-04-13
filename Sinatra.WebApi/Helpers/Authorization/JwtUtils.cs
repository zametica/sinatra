using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Helpers.Authorization;

public interface IJwtUtils
{
    public string GenerateJwtToken(Guid userId, Role role);
    public JwtSecurityToken ValidateJwtToken(string token);
}

public class JwtUtils : IJwtUtils
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly byte[] _key;

    public JwtUtils(IOptions<AppSettings> appSettings)
    {
        var appSettingsValue = appSettings.Value;
        _key = Encoding.ASCII.GetBytes(appSettingsValue.Secret);
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
            Expires = DateTime.UtcNow.AddDays(7), // todo: add expiration in seconds to AppSettings 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public JwtSecurityToken ValidateJwtToken(string token)
    {
        _tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // todo: change to true
            IssuerSigningKey = new SymmetricSecurityKey(_key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        return (JwtSecurityToken) validatedToken;
    }
}