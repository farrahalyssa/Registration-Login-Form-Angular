using Microsoft.AspNetCore.Mvc;
using server.Models;
using MySql.Data.MySqlClient;
using server.Services;

namespace server.Controllers { //basically used for organizing the code, saying it belongs to server/Contollers
    [Route("api/[controller]")]
    [ApiController] //handles HTTP requests

//primary constructor syntax, only works for records and struct types, not normal classes 
    public class RegisterController : ControllerBase { //the apicontroller, t inherits from ControllerBase, which provides HTTP request handling (like GET, POST, etc.).

        private readonly IConfiguration _configuration;

        private readonly JwtService _jwtService;
        public RegisterController(IConfiguration configuration, JwtService jwtService)
        {
            _configuration = configuration;
            _jwtService = jwtService;
        }
        
       [HttpPost]
        [Route("")]
        public IActionResult Register([FromBody] User user)
        {
            

            try
            {
                string? connectionString = _configuration.GetConnectionString("Users");
                string hashedPassword = PasswordService.HashPassword(user.userPassword);


                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    Console.WriteLine($"userName: {user.userName}, userEmail: {user.email}, userPassword: {user.userPassword}, userId: {user.userId}, isActive: {user.isActive}");
                    con.Open();
                    string query = "INSERT INTO User(userName, userId, userPassword, email, isActive) " +
                                   "VALUES(@userName, @userId, @userPassword, @email, @isActive)";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userName", user.userName);
                        cmd.Parameters.AddWithValue("@userId", user.userId);
                        cmd.Parameters.AddWithValue("@userPassword", hashedPassword);
                        cmd.Parameters.AddWithValue("@email", user.email);
                        cmd.Parameters.AddWithValue("@isActive", user.isActive);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            var jwtUser = new JwtUserDto
                            {
                                email = user.email
                            };
                            
                            var token = _jwtService.GenerateJwtToken(jwtUser);
                            return StatusCode(201, new { message = "Data inserted successfully." });
                        }
                        else
                        {
                            return BadRequest(new { message = "Error inserting data." });
                        }
                    }


                }
            }
            catch (Exception ex)
            {
            return StatusCode(500, new { message = $"Error: {ex.Message}", stackTrace = ex.StackTrace });
            }
        


    }}
}
