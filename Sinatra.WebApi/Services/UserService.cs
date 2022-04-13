using Sinatra.Api.Models.Users;

namespace Sinatra.WebApi.Services;

public class UserService : IUserService
{
    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<GetUserResponse> GetUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}
