namespace Sinatra.WebApi.Data.Models;

public class User : BaseEntity<Guid>
{
    public string Identity { get; set; }
    public IdentityType IdentityType { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public Role Role { get; set; }
}
