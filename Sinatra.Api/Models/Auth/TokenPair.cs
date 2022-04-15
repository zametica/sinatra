namespace Sinatra.Api.Models.Auth
{
    public class TokenPair
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
