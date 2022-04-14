using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Helpers.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role[] _roles;
    
    public AuthorizeAttribute(params Role[] roles)
    {
        _roles = roles ?? new Role[] {};
    }

    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userProperties = (UserProperties) context.HttpContext.Items["User"];

        if (userProperties == null || !_roles.Contains(userProperties.Role))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
