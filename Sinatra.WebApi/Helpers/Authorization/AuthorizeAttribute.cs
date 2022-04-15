using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Helpers.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public AuthorizationPolicy Policy { get; set; }
    public Role[] Roles { get; set; }


    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return;
        
        var userProperties = (UserProperties) context.HttpContext.Items["User"];
        
        if (userProperties == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (Policy == AuthorizationPolicy.PERMANENT_USER)
        {
            var permanentUsers = new[] {Role.ADMIN, Role.FACILITY_ADMIN, Role.FACILITY_STAFF, Role.USER};
            if (!permanentUsers.Contains(userProperties.Role))
            {
                context.Result = new UnauthorizedResult();
                return;
            };
        }

        if (Roles is not {Length: > 0}) return;
        
        if (!Roles.Contains(userProperties.Role))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
