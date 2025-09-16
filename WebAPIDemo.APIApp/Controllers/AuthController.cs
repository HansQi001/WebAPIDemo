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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _userService.AuthenticateAsync(dto);

            if (string.IsNullOrEmpty(token)) return Unauthorized();

            return Ok(new { token });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var user = await _userService.CreateAsync(request);

            if (user == null) { return BadRequest(); }

            return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
        }
    }
}
