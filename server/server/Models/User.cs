namespace server.Models{
        public class User{
            public required int UserId {get; set;}
            public required string Name {get; set;}
            public required string Email {get; set;}
            public required string Password {get; set;}
            public required Boolean isActive {get; set;}

        }
    
}