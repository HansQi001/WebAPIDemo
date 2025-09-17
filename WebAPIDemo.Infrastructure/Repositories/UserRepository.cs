using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Application.Common.Interfaces;
using WebAPIDemo.Domain.Entities;
using WebAPIDemo.Infrastructure.Data;

namespace WebAPIDemo.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<bool> ExistsUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username)) { throw new ArgumentNullException(nameof(username)); }

#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) { throw new ArgumentNullException(nameof(email)); }

            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var user = await GetByUsernameAsync(username);

            //if (user == null || !_passwordHasher.Verify(user.PasswordHash, password))
            //{

            //}

            return user;
        }
    }
}
