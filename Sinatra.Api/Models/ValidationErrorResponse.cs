using System.Collections.Generic;

namespace Sinatra.Api.Models
{
    public class ValidationErrorResponse
    {
        public IList<ValidationError> Errors { get; set; }
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
