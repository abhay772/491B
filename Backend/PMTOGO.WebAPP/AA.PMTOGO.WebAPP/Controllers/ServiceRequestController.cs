using AA.PMTOGO.Libary;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AA.PMTOGO.Managers.Interfaces;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController: ControllerBase
    {
        private readonly IServiceRequestManager _requestManager;
        private readonly InputValidation _inputValidation;

        public ServiceRequestController(IServiceRequestManager requestManager, InputValidation inputValidation)
        {
            _requestManager = requestManager;
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
        public async Task<IActionResult> GetServiceRequests()
        {
            Result result = new Result();
            result = ClaimsValidation("Service Provider");
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {
                try
                {
                    Result requests = await _requestManager.GetUserRequests(user.ClaimUsername);
                    if (requests.IsSuccessful)
                    {
                        return Ok(requests.Payload!);
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
        [Route("accept")]
        public async Task<IActionResult> AcceptRequest([FromBody] ServiceInfo service)
        {
            Result result = new Result();
            result = ClaimsValidation("Service Provider");

            if (result.IsSuccessful)
            {
                try
                {
                    Result accept = await _requestManager.AcceptServiceRequest(service.RequestId);
                    if (accept.IsSuccessful)
                    {
                        return Ok(accept.Payload);
                    }
                    else
                    {

                        return BadRequest("Invalid username or password provided. Retry again or contact system admin");
                    }
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }


            }
            return BadRequest("Cookie not found");
        }
        [HttpPost]
        [Route("decline")]
        public async Task<IActionResult> DeclineRequest([FromBody] ServiceInfo service)
        {
            Result result = new Result();
            result = ClaimsValidation("Service Provider");
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {

                try
                {
                    Result removal = await _requestManager.RemoveServiceRequest(service.RequestId, user.ClaimUsername);
                    if (removal.IsSuccessful)
                    {
                        return Ok(removal.Payload);
                    }
                    else
                    {

                        return BadRequest("Invalid username or password provided. Retry again or contact system admin");
                    }
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);

                }

            }
            return BadRequest("Cookie not found");
            
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

        public class ServiceInfo
        {
            public string RequestId { get; set; } = string.Empty;
        }

    }
}

