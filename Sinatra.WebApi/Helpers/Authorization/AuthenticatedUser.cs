using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Helpers.Authorization;

public class AuthenticatedUser
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
}
