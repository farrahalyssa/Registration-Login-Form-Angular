namespace server.Models
{
    public class JwtUser
    {
        public string? userPassword { get; set; }

        public required string email { get; set; }
    }
}
