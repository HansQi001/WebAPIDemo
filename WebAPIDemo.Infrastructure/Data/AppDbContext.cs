using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                // Store normalized values in the DB
                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                // Unique index on normalized username
                entity.HasIndex(u => u.Username)
                      .IsUnique();

                // Unique index on normalized email
                entity.HasIndex(u => u.Email)
                      .IsUnique();
            });
        }

    }

}
