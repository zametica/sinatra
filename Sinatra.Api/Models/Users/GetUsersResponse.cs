using System.Collections.Generic;

namespace Sinatra.Api.Models.Users
{
    public class GetUsersResponse
    {
        public IList<GetUserResponse> Users { get; set; }
    }
}
