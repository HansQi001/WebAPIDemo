using WebAPIDemo.Application.Users.DTOs;

namespace WebAPIDemo.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto?> CreateAsync(CreateUserRequest user);
        Task<UserDto> UpdateAsync(int id, UpdateUserRequest user);
        Task<string?> AuthenticateAsync(LoginDTO dto);
    }
}
