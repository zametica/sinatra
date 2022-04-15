namespace Sinatra.WebApi.Data.Models;

public class RefreshToken : BaseEntity<long>
{
    public string Token { get; set; }
    public bool Valid { get; set; }

    // navigation
    public long TokenFamilyId { get; set; }
    public TokenFamily TokenFamily { get; set; }
}
