namespace Sinatra.ApiClient.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IList<ValidationError> errors)
    {
        this.Errors = errors;
    }

    public IList<ValidationError> Errors { get; set; }
}

public class ValidationError
{
    public string Field { get; set; }
    public string Message { get; set; }
}
