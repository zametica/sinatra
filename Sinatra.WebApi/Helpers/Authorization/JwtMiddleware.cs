using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Helpers.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtUtils _jwtUtils;

    public JwtMiddleware(RequestDelegate next, IJwtUtils jwtUtils)
    {
        _next = next;
        _jwtUtils = jwtUtils;
    }


    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            AttachUserToContext(context, token);
        }
        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var validatedToken = _jwtUtils.ValidateJwtToken(token);
            
            var authenticatedUser = new AuthenticatedUser()
            {
                Id = Guid.Parse(validatedToken.Claims.First(x => x.Type == "user_id").Value),
                Role = (Role) Enum.Parse(typeof(Role), validatedToken.Claims.First(x => x.Type == "role").Value)
            };

            context.Items["User"] = authenticatedUser;
        }
        catch
        {
            // Ignore exception. Without UserContext, AuthorizationFilter will return 401
        }
    }
}

