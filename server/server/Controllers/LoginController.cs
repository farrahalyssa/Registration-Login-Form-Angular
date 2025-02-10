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
        public IActionResult Login([FromBody] User loginUser)
        {
            try
            {
                string? connectionString = _configuration.GetConnectionString("Users");

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT userName, userPassword, email, isActive FROM User WHERE userEmail = @userEmail";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userEmail", loginUser.userEmail);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve the stored hashed password from the database
                                string? storedHash = reader["userPassword"]?.ToString();

                                // Check if storedHash is null or empty
                                if (string.IsNullOrEmpty(storedHash))
                                {
                                    return Unauthorized(new { message = "Invalid credentials." });
                                }

                                // Verify the entered password against the stored hash
                                bool isPasswordValid = PasswordService.VerifyPassword(loginUser.userPassword, storedHash);

                                if (isPasswordValid)
                                {
                                    // Create a User object without the userPassword field
                                    var user = new User
                                    {
                                        userId = reader["userId"]?.ToString() ?? string.Empty,
                                        userName = reader["userName"]?.ToString() ?? string.Empty,
                                        userPassword = reader["userPassword"]?.ToString() ?? string.Empty, 
                                        userEmail = reader["email"]?.ToString() ?? string.Empty,
                                        isActive = reader["isActive"] != DBNull.Value ? Convert.ToBoolean(reader["isActive"]) : false
                                    };

                                    // Generate the JWT token
                                    var token = _jwtService.GenerateJwtToken(user);

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
