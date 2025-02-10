namespace server.Models
{
    public class JwtUserDto
    {
        public string? userPassword { get; set; }

        public required string email { get; set; }
    }
}
