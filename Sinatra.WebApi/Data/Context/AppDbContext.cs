using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sinatra.WebApi.Data.Models;
using Sinatra.WebApi.Helpers.Authorization;

namespace Sinatra.WebApi.Data.Context;

public class AppDbContext : DbContext
{
    private readonly UserContext _userContext;

    public AppDbContext(DbContextOptions<AppDbContext> options, UserContext userContext) : base(options)
    {
        _userContext = userContext;
    }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().Property(u => u.Role).HasConversion(new EnumToStringConverter<Role>());
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(s => s.Entity is BaseEntity))
        {
            var entity = (BaseEntity) entry.Entity;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = DateTimeOffset.Now;
                    entity.CreatedBy = _userContext.Properties?.Id;
                    break;
                case EntityState.Modified:
                    entity.UpdatedAt = DateTimeOffset.Now;
                    entity.UpdatedBy = _userContext.Properties?.Id;
                    break;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
