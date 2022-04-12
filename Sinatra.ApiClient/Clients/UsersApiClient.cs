using Sinatra.Api.Apis;
using Sinatra.Api.Models.Users;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sinatra.ApiClient.Clients
{
    public class UsersApiClient : IUsersApi
    {
        private readonly SimpleClient _client;

        public UsersApiClient(SimpleClient simpleClient)
        {
            _client = simpleClient;
        }


        public async Task<GetUserResponse> GetUserAsync(Guid userId)
        {
            return await _client.Invoke<GetUserResponse>(HttpMethod.Get, new Uri($"/api/users/{userId}"));
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest body)
        {
            return await _client.Invoke<CreateUserResponse>(HttpMethod.Post, new Uri("/api/users"), body);
        }
    }
}
