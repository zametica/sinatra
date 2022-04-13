﻿using Microsoft.EntityFrameworkCore;
using Sinatra.Api.Models.Users;
using Sinatra.WebApi.Data.Context;
using Sinatra.WebApi.Data.Models;
using Sinatra.WebApi.Helpers.Authorization;
using Sinatra.WebApi.Helpers.Utils;
using Role = Sinatra.WebApi.Data.Models.Role;

namespace Sinatra.WebApi.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserContext _userContext;
    private readonly AppDbContext _db;

    public UserService(ILogger<UserService> logger, UserContext userContext, AppDbContext db)
    {
        _logger = logger;
        _userContext = userContext;
        _db = db;
    }


    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Role.USER 
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new CreateUserResponse
        {
            Id = user.Id
        };
    }

    public async Task<GetUserResponse> GetUserAsync(Guid userId)
    {
        return await _db.Users.Select(x => new GetUserResponse
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Role = (Api.Models.Users.Role) Enum.Parse(typeof(Api.Models.Users.Role), x.Role.ToString())
        }).FirstOrDefaultAsync();
    }
}
