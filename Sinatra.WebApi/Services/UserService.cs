using Microsoft.EntityFrameworkCore;
using Sinatra.Api.Models.Users;
using Sinatra.WebApi.Data.Context;
using Sinatra.WebApi.Data.Models;
using Sinatra.WebApi.Helpers.Authorization;

namespace Sinatra.WebApi.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IJwtUtils _jwtUtils;
    private readonly UserContext _userContext;
    private readonly AppDbContext _db;

    public UserService(IJwtUtils jwtUtils, ILogger<UserService> logger, UserContext userContext, AppDbContext db)
    {
        _logger = logger;
        _jwtUtils = jwtUtils;
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
            PasswordHash = "encoded" + request.Password,
            Role = (Data.Models.Role) Enum.Parse(typeof(Data.Models.Role), request.Role.ToString())   
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
    
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.Where(x => x.Email.Equals(request.Email)).FirstOrDefaultAsync();
        if (user != null)
        {
            return new LoginResponse { Token = _jwtUtils.GenerateJwtToken(user.Id, user.Role) };
        }

        throw new Exception();
    }
}
