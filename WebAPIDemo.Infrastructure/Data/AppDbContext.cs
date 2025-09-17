using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Application.Common.Interfaces;
using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IPasswordHasher _passwordHaser;

        public AppDbContext(DbContextOptions<AppDbContext> options
            , IPasswordHasher passwordHasher)
            : base(options) { _passwordHaser = passwordHasher; }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

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

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Hans", PasswordHash = _passwordHaser.Hash("1234"), Email = "qihang001@outlook.com" });

            var products = Enumerable.Range(1, 100)
                    .Select(i => new Product
                    {
                        Id = i,
                        Name = $"Product {i}",
                        Price = Math.Round((decimal)(i * 0.5), 2),
                        Stock = i % 100
                    })
                    .ToArray();

            modelBuilder.Entity<Product>().HasData(products);

        }

    }

}
