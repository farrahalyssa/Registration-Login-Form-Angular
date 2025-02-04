using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using server.Models;
using MySql.Data.MySqlClient;
using System.Data;


namespace server.Controllers { //basically used for organizing the code, saying it belongs to server/Contollers
    [Route("api/[controller]")]
    [ApiController] //handles HTTP requests

//primary constructor syntax, only works for records and struct types, not normal classes 
    public class AuthController : ControllerBase { //the apicontroller, t inherits from ControllerBase, which provides HTTP request handling (like GET, POST, etc.).
        // private readonly MySqlConnectionService _mySqlConnectionService;

        private readonly IConfiguration _configuration;
        // public AuthController(MySqlConnectionService mySqlConnectionService)
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            // _mySqlConnectionService = mySqlConnectionService;
        }
        
       [HttpPost]
        [Route("registration")]
        public string Registration(User user)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("Users");
                if (string.IsNullOrEmpty(connectionString))
                {
                    return "Connection string is missing or incorrect.";
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO Users(userName, userId, userPassword, email, isActive) " +
                                   "VALUES(@userName, @userId, @userPassword, @email, 1)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userName", user.Name);
                        cmd.Parameters.AddWithValue("@userId", user.UserId);
                        cmd.Parameters.AddWithValue("@userPassword", user.Password);
                        cmd.Parameters.AddWithValue("@email", user.Email);

                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            return "Data inserted successfully.";
                        }
                        else
                        {
                            return "Error inserting data.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        
    

        // public string login(Registration registration){
            //sql connection
            //sql data adapter ??
            // what is a datable 


            // valid user invalid user why dt.rows >0 ?? i dont get it

        // }

    }}
}