using Sinatra.WebApi.Data.Models;
using Sinatra.WebApi.Helpers.Utils;

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
            context.Items["User"] = _jwtUtils.ValidateJwtToken(token);
        }
        catch
        {
            // Ignore exception. Without UserContext, AuthorizationFilter will return 401
        }
    }
}
