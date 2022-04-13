namespace Sinatra.WebApi.Helpers.Authorization;

public class UserContext
{
    public UserContext(IHttpContextAccessor contextAccessor)
    {
        AuthenticatedUser = (AuthenticatedUser) contextAccessor.HttpContext!.Items["User"];
    }
    
    public AuthenticatedUser AuthenticatedUser { get; }
}