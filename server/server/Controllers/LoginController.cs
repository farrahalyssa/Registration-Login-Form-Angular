using Microsoft.AspNetCore.Mvc;
using server.Models;
using MySql.Data.MySqlClient;
using System.Configuration;


namespace server.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase{
    
    private readonly IConfiguration _configuration;

    public LoginController(IConfiguration configuration){
        _configuration = configuration;
    }

    [HttpPost]
    [Route("")]
    public IActionResult Login(User user){
        string? connectionString =  _configuration.GetConnectionString("Users");

         if (user.userEmail == "test@example.com" && user.userPassword == "password123")
            {
                return Ok(new { message = "Login successful", user = user });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
    
}