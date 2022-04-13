using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sinatra.WebApi.Data.Models;

namespace Sinatra.WebApi.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().Property(u => u.Role).HasConversion(new EnumToStringConverter<Role>());
    }

    public DbSet<User> Users { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(s => s.Entity is BaseEntity))
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    (entry.Entity as BaseEntity).CreatedAt = DateTimeOffset.Now;
                    break;
                case EntityState.Modified:
                    (entry.Entity as BaseEntity).UpdatedAt = DateTimeOffset.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
