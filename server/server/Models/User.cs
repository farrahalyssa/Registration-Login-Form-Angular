namespace server.Models{
        public class User{
            public required int userId {get; set;}
            public required string userName {get; set;}
            public required string userEmail {get; set;}
            public required string userPassword {get; set;}
            public required Boolean isActive {get; set;}

        }
    
        public enum AccountStatus {
        Active = 1,
        Inactive = 0
        
        }

}

  // "ConnectionStrings": {
    
  // },