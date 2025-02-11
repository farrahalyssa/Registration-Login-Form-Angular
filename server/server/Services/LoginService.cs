using MySql.Data.MySqlClient;
using server.Models;

namespace server.Services
{
    public class LoginService
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtUser? AuthenticateUser(string email, string password)
        {
            string? connectionString = _configuration.GetConnectionString("Users");

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT userId, userName, email, userPassword, isActive FROM User WHERE LOWER(email) = LOWER(@email)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string? storedHash = reader["userPassword"] as string;
                            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(password))
                                return null;

                            bool isPasswordValid = PasswordService.VerifyPassword(password, storedHash);
                            if (!isPasswordValid)
                                return null;

                            return new JwtUser
                            {
                                email = reader["email"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
