using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace server.Controllers { //basically used for organizing the code, saying it belongs to server/Contollers
    [Route("api/[controller]")]
    [ApiController] //handles HTTP requests

//primary constructor syntax, only works for records and struct types, not normal classes 
    public class UserController : ControllerBase { //the apicontroller, t inherits from ControllerBase, which provides HTTP request handling (like GET, POST, etc.).
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    }
}