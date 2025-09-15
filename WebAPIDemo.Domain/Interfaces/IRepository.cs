namespace WebAPIDemo.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task<int> SaveChangesAsync();
        Task<bool> ExistAsync(int id);
        Task<int> DeleteByIdAsync(int id);
        Task AddRangeAsync(IEnumerable<T> entities);
    }
}
