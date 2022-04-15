using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Helpers.Authorization;

public class UserProperties
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
}
