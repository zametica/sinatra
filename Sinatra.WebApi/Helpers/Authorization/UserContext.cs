namespace Sinatra.WebApi.Helpers.Authorization;

public class UserContext
{
    public UserContext(IHttpContextAccessor contextAccessor)
    {
        Properties = (UserProperties) contextAccessor.HttpContext!.Items["User"];
    }
    
    public UserProperties Properties { get; }
}
