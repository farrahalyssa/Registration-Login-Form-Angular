using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterService _registerService;
        private readonly JwtService _jwtService;

        public RegisterController(RegisterService registerService, JwtService jwtService)
        {
            _registerService = registerService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                bool isRegistered = _registerService.RegisterUser(user);

                if (isRegistered)
                {
                    var jwtUser = new JwtUser { email = user.email };
                    var token = _jwtService.GenerateJwtToken(jwtUser);
                    return StatusCode(201, new { message = "Data inserted successfully.", token = token });
                }
                else
                {
                    return BadRequest(new { message = "Error inserting data." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}", stackTrace = ex.StackTrace });
            }
        }
    }
}
