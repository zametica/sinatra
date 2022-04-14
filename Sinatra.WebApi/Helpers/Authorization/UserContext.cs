namespace Sinatra.WebApi.Helpers.Authorization;

public class UserContext
{
    public UserContext(IHttpContextAccessor contextAccessor)
    {
        UserProperties = (UserProperties) contextAccessor.HttpContext!.Items["User"];
    }
    
    public UserProperties UserProperties { get; }
}
