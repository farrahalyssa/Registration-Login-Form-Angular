using System.ComponentModel.DataAnnotations;

namespace server.Models{
        public class User{
           
            public required string userName {get; set;}
            public required string userId {get; set;}
            [EmailAddress]
            public required string email {get; set;}

            public required string userPassword {get; set;}
            public required Boolean isActive {get; set;}


        }
    
      

}

 