using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.Application.Common.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> ValidateUserAsync(string username, string password);
        Task<bool> ExistsUsernameAsync(string username);
    }
}
