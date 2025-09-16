using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Application.Common.Interfaces;
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

            var entity = Activator.CreateInstance<T>();
            _context.Entry(entity).Property("Id").CurrentValue = id;

            _dbSet.Remove(entity);

            return await SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }

}
