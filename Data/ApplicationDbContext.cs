namespace AbpTask.Data;

using AbpTask.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<RoomEntity> Rooms { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServiceEntity>()
            .HasOne(service => service.Room)
            .WithMany(room => room.Services)
            .HasForeignKey(service => service.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IBaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var auditable = (IBaseEntity)entityEntry.Entity;

            var now = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                auditable.CreatedAt = now;
            }

            auditable.UpdatedAt = now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
