namespace Sinatra.WebApi.Helpers.Configuration;

public class JwtConfiguration
{
    public string Secret { get; set; }
    public long TokenValidityInSeconds { get; set; }
    public long RefreshTokenValidityInDays { get; set; }
}
