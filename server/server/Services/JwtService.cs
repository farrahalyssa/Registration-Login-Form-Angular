using server.Models;

namespace server.Services{
    public class JwtService{

        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration){
            _configuration = configuration;

        }

        // public async Task<LoginResponseModel>Authenticate(LoginRequestModel request){
        //     if(string.IsNullOrWhiteSpace(request.userEmail) || string.IsNullOrWhiteSpace(request.userPassword)){
        //         return null;
        //     }
        // }
    }
}