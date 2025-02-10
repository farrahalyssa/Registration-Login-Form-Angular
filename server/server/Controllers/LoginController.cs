using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using server.Models;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;

        public LoginController(IConfiguration configuration, JwtService jwtService)
        {
            _configuration = configuration;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("")]
        public IActionResult Login([FromBody] JwtUserDto user)
        {
            try
            {
                string? connectionString = _configuration.GetConnectionString("Users");

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT userId, userName, email, userPassword, isActive FROM User WHERE LOWER(email) = LOWER(@email)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@email", user.email);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string? storedHash = reader["userPassword"] as string;
                                if (string.IsNullOrEmpty(storedHash))
                                {
                                    return Unauthorized(new { message = "Invalid credentials." });
                                }

                                if (string.IsNullOrEmpty(user.userPassword) || string.IsNullOrEmpty(storedHash))
                                {
                                    return Unauthorized(new { message = "Invalid credentials." });
                                }

                                bool isPasswordValid = PasswordService.VerifyPassword(user.userPassword!, storedHash);

                                if (isPasswordValid)
                                {
                                      var jwtUser = new JwtUserDto
                                    {
                                        email = reader["email"]?.ToString() ?? string.Empty
                                    };

                                    var token = _jwtService.GenerateJwtToken(jwtUser);

                                    return Ok(new { message = "Login successful.", token = token });
                                }
                                else
                                {
                                    return Unauthorized(new { message = "Invalid credentials." });
                                }
                            }
                            else
                            {
                                return NotFound(new { message = "User not found." });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }
    }
}
