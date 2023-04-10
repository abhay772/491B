
using AA.PMTOGO.Libary;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly InputValidation _inputValidation;

        public ServiceController(IServiceManager serviceManager, InputValidation inputValidation)
        {
            _serviceManager = serviceManager;
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
        [Route("getuserservice")]
        [Consumes("application/json", "application/problem+json")]
        public async Task<IActionResult> GetUserService()
        {
            Result result = new Result();
            result = ClaimsValidation(null!);
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {
                try
                {
                    Result userServices = await _serviceManager.GetAllUserServices(user.ClaimUsername);
                    if (userServices.IsSuccessful)
                    {
                        return Ok(userServices.Payload!);
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
            return BadRequest("Cookie not found");
        }

        [HttpGet]
        [Route("getservice")]
        [Consumes("application/json", "application/problem+json")]
        public async Task<IActionResult> GetServices()
        {
            Result result = new Result();
            result = ClaimsValidation(null!);

            if (result.IsSuccessful)
            {
                try
                {
                    Result services = await _serviceManager.GetAllServices();
                    if (services.IsSuccessful)
                    {
                        return Ok(services.Payload!);
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
            return BadRequest("Not Authorized");
            
        }

        [HttpPost]
        [Route("addrequests")]
        public async Task<IActionResult> AddServiceRequest(Service service, string comments, string frequency)
        {
            Result result = new Result();
            result = ClaimsValidation("Property Manager");
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {
                try
                {
                    Result insert = await _serviceManager.AddServiceRequest(service, user!.ClaimUsername, comments, frequency);
                    if (insert.IsSuccessful)
                    {
                        return Ok(new { message = insert.Payload});
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
            return BadRequest("Invalid Credentials");
        }
        
        [HttpPut]
        [Route("{rate}")]
        public async Task<IActionResult> RateService([FromBody] UserService service, int rate)
        {
            Result result = new Result();
            result = ClaimsValidation(null!);

            if (result.IsSuccessful )
            {
                try
                {
                    Result rating = await _serviceManager.RateUserService(service, rate);
                    if (rating.IsSuccessful)
                    {
                        return Ok(rating.Payload);
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
            return BadRequest("Invalid Credentials");
        }

        private Result ClaimsValidation(string role)
        {
            Result result = new Result();
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
                        result.IsSuccessful = false;
                        result.ErrorMessage = "Invalid Claims";
                        return result;
                    }

                    var claims = jwtToken.Claims.ToList();
                    Claim usernameClaim = claims[0];
                    Claim roleClaim = claims[1];

                    if (usernameClaim != null && roleClaim != null)
                    {
                        string username = usernameClaim.Value;
                        string userrole = roleClaim.Value;

                        // Check if the role is Property Manager
                        bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;
                        if (validationCheck && userrole == role || role == null)
                        {
                            UserClaims user = new UserClaims(username, role!);

                            result.IsSuccessful = true;
                            result.Payload = user;
                            return result;
                        }
                    }
                    return result;
                }
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid Claims";
                return result;
            }
            result.IsSuccessful = false;
            return result;
        }

    }
}
