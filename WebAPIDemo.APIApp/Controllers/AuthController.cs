using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Application.Common.Interfaces;
using WebAPIDemo.Application.Users.DTOs;

namespace WebAPIDemo.APIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _userService.AuthenticateAsync(dto);

            if (string.IsNullOrEmpty(token)) return Unauthorized();

            return Ok(new { token });
        }
    }
}
