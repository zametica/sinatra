using Sinatra.Api.Models.Users;

namespace Sinatra.WebApi.Services;

public interface IUserService
{
    public Task<GetUserResponse> GetUserAsync(Guid userId);
    public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
}
