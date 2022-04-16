namespace Sinatra.Api.Models.Users
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
