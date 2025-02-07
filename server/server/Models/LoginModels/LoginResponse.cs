
namespace server.Models{
public class LoginResponseModel{

    public string? userEmail {get; set;}
    public string? AccessToken {get; set;}
    public int ExpiresIn {get; set;}
}
}