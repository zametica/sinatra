using Microsoft.EntityFrameworkCore;
using Sinatra.Api.Models.Users;
using Sinatra.WebApi.Data.Context;
using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    
    public UserService(AppDbContext db)
    {
        _db = db;
    }


    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = "encoded" + request.Password
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
            LastName = x.LastName
        }).FirstOrDefaultAsync();
    }
}
