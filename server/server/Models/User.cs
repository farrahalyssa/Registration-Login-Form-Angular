using System.ComponentModel.DataAnnotations;

namespace server.Models{
        public class User{
            [Key]
            public required int userId {get; set;}
            public required string userName {get; set;}
            [EmailAddress]
            public required string userEmail {get; set;}
            [MinLength(8)]

            public required string userPassword {get; set;}
            public required Boolean isActive {get; set;}

            public DateTime CreatedAt { get; set; } 

        }
    
      

}

 