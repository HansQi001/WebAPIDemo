using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
    }

}
