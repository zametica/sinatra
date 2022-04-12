using Sinatra.Api.Models.Users;
using System;
using System.Threading.Tasks;

namespace Sinatra.Api.Apis
{
    public interface IUsersApi
    {
        Task<GetUserResponse> GetUserAsync(Guid userId);
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest body);
    }
}
