using System.ComponentModel.DataAnnotations;

namespace server.Models{
    public class LoginRequestModel{
        [Required, EmailAddress]
        public string? userEmail{get; set;}

        [Required, MinLength(8)]
        public string? userPassword{get; set;}

    }
}