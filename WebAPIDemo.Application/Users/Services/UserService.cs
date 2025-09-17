using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Application.Common.Interfaces;
using WebAPIDemo.Application.Users.DTOs;
using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.Application.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            IAuthService authService,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task<string?> AuthenticateAsync(LoginDTO dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null) return null;

            var passwordValid = _passwordHasher.Verify(user.PasswordHash, dto.Password);
            if (!passwordValid) return null;

            return _authService.GenerateJwtToken(user);
        }

        public async Task<UserDto?> CreateAsync(CreateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Username, email, and password are required.");
            }

            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (await _userRepository.GetByUsernameAsync(request.Username) != null
                || await _userRepository.GetByEmailAsync(request.Email) != null) return null;

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            try
            {
                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                // Another request inserted the same username/email in parallel
                return null;
            }

            return new UserDto(user.Id, user.Username, user.Email);
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            // Check inner exception or SQL error code depending on DB provider
            return ex.InnerException?.Message.Contains("IX_Users_Username_CI") == true
                || ex.InnerException?.Message.Contains("IX_Users_Email_CI") == true;
        }


        public Task<UserDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> UpdateAsync(int id, UpdateUserRequest user)
        {
            throw new NotImplementedException();
        }
    }
}
