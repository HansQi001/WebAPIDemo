using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Domain.Interfaces;
using WebAPIDemo.Infrastructure.Data;

namespace WebAPIDemo.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public async Task<bool> ExistAsync(int id)
            => await _dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);

        public async Task<int> DeleteByIdAsync(int id)
        {
            if (!await ExistAsync(id))
                return 0;

            _dbSet.Remove((T)Activator.CreateInstance(typeof(T), new object[] { })!);
            _context.Entry(_dbSet.Local.Last()).Property("Id").CurrentValue = id;

            return await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }

}
