namespace Sinatra.WebApi.Data.Models;

public class TokenFamily : BaseEntity<long>
{
    public DateTimeOffset ExpirationTime { get; set; }
    
    // navigation
    public Guid UserId { get; set; }
    public User User { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
}
