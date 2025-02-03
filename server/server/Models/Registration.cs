namespace server.Models{
        public class Registration{
            public required int UserId {get; set;}
            public required string Name {get; set;}
            public required string Email {get; set;}
            public required string Password {get; set;}
            public required Boolean isActive {get; set;}

        }
    
        public enum AccountStatus {
        Active = 1,
        Inactive = 0
        
        }

}

  // "ConnectionStrings": {
    
  // },