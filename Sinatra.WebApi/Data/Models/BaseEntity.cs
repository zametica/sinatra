namespace Sinatra.WebApi.Data.Models;

public class BaseEntity<T> : BaseEntity 
{
    public T Id { get; set; }
}

public class BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}
