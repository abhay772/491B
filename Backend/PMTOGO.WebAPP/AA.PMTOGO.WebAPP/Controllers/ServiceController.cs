using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Libary;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ILogger = AA.PMTOGO.Infrastructure.Interfaces.ILogger;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        //private readonly UsersDbContext usersDbContext;
        private readonly IServiceManager _serviceManager;
        private readonly InputValidation _inputValidation;
        private readonly ILogger _logger;

        public ServiceController(IServiceManager serviceManager, ILogger logger, InputValidation inputValidation)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _inputValidation = inputValidation;
        }
#if DEBUG
        [HttpGet]
        [Route("health")]   //make sure controller route works.
        public Task<IActionResult> HealthCheck()
        {
            return Task.FromResult<IActionResult>(Ok("Healthy"));
        }
#endif
        [HttpGet]
        [Route("getrequest")]
        [Consumes("application/json", "application/problem+json")]
        public async Task<IActionResult> GetRequest()
        {
            try
            {
                // Loading the cookie from the http request
                var cookieValue = Request.Cookies["CredentialCookie"];

                if (!string.IsNullOrEmpty(cookieValue))
                {
                    var handler = new JwtSecurityTokenHandler();

                    var jwtToken = handler.ReadJwtToken(cookieValue);

                    if (jwtToken == null)
                    {
                        return BadRequest("Invalid Claims");
                    }

                    var claims = jwtToken.Claims.ToList();
                    Claim usernameClaim = claims[0];
                    Claim roleClaim = claims[1];


                    if (usernameClaim != null && roleClaim != null)
                    {
                        string username = usernameClaim.Value;
                        string role = roleClaim.Value;

                        // Check if the role is Property Manager
                        bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                        if (role != null && validationCheck && role == "Service Provider")
                        {
                            Result result = new Result();
                            try
                            {
                                result = await _serviceManager.GetUserRequest(username);
                                if (result.IsSuccessful)
                                {
                                    return Ok(new { message = result.Payload });
                                }
                                else
                                {
                                    return BadRequest(new { message = "Retry again or contact system admin." });
                                }
                            }
                            catch
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError);
                            }
                        }
                    }

                }

                return BadRequest("Cookie not found");
            }

            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("addrequests")]
        public async Task<IActionResult> AddServiceRequest(ServiceRequest serviceRequest)
        {
            try
            {

                // Loading the cookie from the http request
                var cookieValue = Request.Cookies["CredentialCookie"];

                if (!string.IsNullOrEmpty(cookieValue))
                {
                    var handler = new JwtSecurityTokenHandler();

                    var jwtToken = handler.ReadJwtToken(cookieValue);

                    if (jwtToken == null)
                    {
                        return BadRequest("Invalid Claims");
                    }

                    var claims = jwtToken.Claims.ToList();
                    Claim usernameClaim = claims[0];
                    Claim roleClaim = claims[1];

                    if (usernameClaim != null && roleClaim != null)
                    {
                        string username = usernameClaim.Value;
                        string role = roleClaim.Value;

                        // Check if the role is Property Manager
                        bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                        if (role != null && validationCheck && role == "Property Manager")
                        {
                            try
                            {
                                Result result = await _serviceManager.RequestAService(serviceRequest);
                                if (result.IsSuccessful)
                                {
                                    return Ok(new { message = result.Payload});
                                }
                                else
                                {

                                    return BadRequest(new { message = "Retry again or contact system admin" } );
                                }
                            }
                            catch
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError);

                            }

                        }

                    }
                }
                    
                return BadRequest("Cookie not found");
                   
            }

            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        [HttpPost]
        [Route("accept")]
        public async Task<IActionResult> AcceptRequest([FromBody] ServiceRequest serviceRequest)
        {
            try
            {

                // Loading the cookie from the http request
                var cookieValue = Request.Cookies["CredentialCookie"];

                if (!string.IsNullOrEmpty(cookieValue))
                {
                    var handler = new JwtSecurityTokenHandler();

                    var jwtToken = handler.ReadJwtToken(cookieValue);

                    if (jwtToken == null)
                    {
                        return BadRequest("Invalid Claims");
                    }

                    var claims = jwtToken.Claims.ToList();
                    Claim usernameClaim = claims[0];
                    Claim roleClaim = claims[1];

                    if (usernameClaim != null && roleClaim != null)
                    {
                        string username = usernameClaim.Value;
                        string role = roleClaim.Value;

                        // Check if the role is Property Manager
                        bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                        if (role != null && validationCheck && role == "Service Provider")
                        {
                            try
                            {
                                Result result = await _serviceManager.AcceptServiceRequest(serviceRequest);
                                if (result.IsSuccessful)
                                {
                                    return Ok(result.Payload);
                                }
                                else
                                {

                                    return BadRequest("Invalid username or password provided. Retry again or contact system admin" + result.Payload);
                                }
                            }
                            catch
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError);
                            }

                        }

                    }

                }            
                return BadRequest("Cookie not found");
            }

            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("decline")]
        public async Task<IActionResult> DeclineRequest([FromBody] ServiceRequest serviceRequest)
        {
            try
            {

                // Loading the cookie from the http request
                var cookieValue = Request.Cookies["CredentialCookie"];

                if (!string.IsNullOrEmpty(cookieValue))
                {
                    var handler = new JwtSecurityTokenHandler();

                    var jwtToken = handler.ReadJwtToken(cookieValue);

                    if (jwtToken == null)
                    {
                        return BadRequest("Invalid Claims");
                    }

                    var claims = jwtToken.Claims.ToList();
                    Claim usernameClaim = claims[0];
                    Claim roleClaim = claims[1];

                    if (usernameClaim != null && roleClaim != null)
                    {
                        string username = usernameClaim.Value;
                        string role = roleClaim.Value;

                        // Check if the role is Property Manager
                        bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                        if (role != null && validationCheck && role == "Service Provider")
                        {

                            try
                            {
                                Result result = await _serviceManager.RemoveServiceRequest(serviceRequest);
                                if (result.IsSuccessful)
                                {
                                    return Ok(result.Payload);
                                }
                                else
                                {

                                    return BadRequest("Invalid username or password provided. Retry again or contact system admin" + result.Payload);
                                }
                            }
                            catch
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError);
        
                            }
                        }

                    }

                }
                return BadRequest("Cookie not found");
            }

            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("{rate}")]
        public async Task<IActionResult> RateService([FromBody] UserService service, int rate)
        {
            try
            {

                // Loading the cookie from the http request
                var cookieValue = Request.Cookies["CredentialCookie"];

                if (!string.IsNullOrEmpty(cookieValue))
                {
                    var handler = new JwtSecurityTokenHandler();

                    var jwtToken = handler.ReadJwtToken(cookieValue);

                    if (jwtToken == null)
                    {
                        return BadRequest("Invalid Claims");
                    }

                    var claims = jwtToken.Claims.ToList();
                    Claim usernameClaim = claims[0];
                    Claim roleClaim = claims[1];

                    if (usernameClaim != null && roleClaim != null)
                    {
                        string username = usernameClaim.Value;
                        string role = roleClaim.Value;

                        // Check if the role is Property Manager
                        bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                        if (role != null && validationCheck && role == "Property Manager")
                        {

                            try
                            {
                                Result result = await _serviceManager.RateUserService(service, rate);
                                if (result.IsSuccessful)
                                {
                                    return Ok(result.Payload);
                                }
                                else
                                {

                                    return BadRequest("Invalid username or password provided. Retry again or contact system admin" + result.Payload);
                                }
                            }
                            catch
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError);
                            }


                        }
                    }
                }
                return BadRequest("Cookie not found");
                
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Route("addservice")]
        public async Task<IActionResult> AddUserService([FromBody] UserService userService)
        {
            try
            {
                Result result = await _serviceManager.AddServiceToUser(userService);
                if (result.IsSuccessful)
                {
                    return Ok(result.Payload);
                }
                else
                {

                    return BadRequest("Invalid username or password provided. Retry again or contact system admin" + result.Payload);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
