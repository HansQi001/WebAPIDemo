using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.Application.Common.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
    }
}
