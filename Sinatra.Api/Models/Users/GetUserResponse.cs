using System;

namespace Sinatra.Api.Models.Users
{
    public class GetUserResponse
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
    }
}
