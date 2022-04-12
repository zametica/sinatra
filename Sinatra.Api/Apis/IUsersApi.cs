using Sinatra.Api.Models.Users;

namespace Sinatra.Api.Apis;
public interface IUsersApi
{
    public Task<GetUserResponse> GetUserAsync(Guid userId);
    public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest body);
}
