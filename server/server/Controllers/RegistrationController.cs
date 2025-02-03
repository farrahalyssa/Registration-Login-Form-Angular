using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using server.Models;

// put database connection in .db file and configuration, then import it  
namespace server.Controllers { //basically used for organizing the code, saying it belongs to server/Contollers
    [Route("api/[controller]")]
    [ApiController] //handles HTTP requests

//primary constructor syntax, only works for records and struct types, not normal classes 
    public class RegistrationController : ControllerBase { //the apicontroller, t inherits from ControllerBase, which provides HTTP request handling (like GET, POST, etc.).
        private readonly IConfiguration _configuration;

        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpPost]
        [Route("registration")]

        public string registration(Registration registration){
            //sql connection
            //sql command
            return "";
        }

        public string login(Registration registration){
            //sql connection
            //sql data adapter ??
            // what is a datable 


            // valid user invalid user why dt.rows >0 ?? i dont get it

        }

    }
}