namespace Sinatra.WebApi.Data.Models;

public class UserSession : BaseEntity<long>
{
    public string RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpirationTime { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
}
