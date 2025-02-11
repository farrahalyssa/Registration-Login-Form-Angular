
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly JwtService _jwtService;

        public LoginController(LoginService loginService, JwtService jwtService)
        {
            _loginService = loginService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("")]
        public IActionResult Login([FromBody] JwtUser user)
        {
               if (string.IsNullOrEmpty(user.userPassword))
                {
                    return Unauthorized(new { message = "Invalid credentials." });
                }
            try
            {
                var authenticatedUser = _loginService.AuthenticateUser(user.email, user.userPassword);

                if (authenticatedUser == null)
                {
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                var token = _jwtService.GenerateJwtToken(authenticatedUser);
                return Ok(new { message = "Login successful.", token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }
    }
}
