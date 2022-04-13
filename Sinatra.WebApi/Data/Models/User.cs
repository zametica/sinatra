namespace Sinatra.WebApi.Data.Models;

public class User : BaseEntity<Guid>
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
