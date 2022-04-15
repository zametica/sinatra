using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion(new EnumToStringConverter<Role>());
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TokenFamily> TokenFamilies { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(s => s.Entity is BaseEntity))
        {
            var entity = (BaseEntity) entry.Entity;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = DateTimeOffset.Now;
                    break;
                case EntityState.Modified:
                    entity.UpdatedAt = DateTimeOffset.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
