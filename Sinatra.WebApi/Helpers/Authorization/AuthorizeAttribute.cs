﻿using Microsoft.AspNetCore.Mvc;
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
        var user = (AuthenticatedUser) context.HttpContext.Items["User"];

        if (user == null || !_roles.Contains(user.Role))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
