using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sinatra.Api.Apis;
using Sinatra.Api.Models.Users;

namespace Sinatra.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, IUsersApi
    {
        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest body)
        {
            throw new NotImplementedException();
        }

        public async Task<GetUserResponse> GetUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
