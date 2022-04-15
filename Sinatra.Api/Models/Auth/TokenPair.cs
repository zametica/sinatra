namespace Sinatra.Api.Models.Auth
{
    public abstract class TokenPair
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
