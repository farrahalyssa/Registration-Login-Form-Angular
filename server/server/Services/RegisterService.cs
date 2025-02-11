using MySql.Data.MySqlClient;
using server.Models;

namespace server.Services
{
    public class RegisterService
    {
        private readonly IConfiguration _configuration;

        public RegisterService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool RegisterUser(User user)
        {
            string? connectionString = _configuration.GetConnectionString("Users");
            string hashedPassword = PasswordService.HashPassword(user.userPassword);

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
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
                    return result > 0;
                }
            }
        }
    }
}
