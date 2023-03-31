using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Libary;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController: ControllerBase
    {
        private readonly IRequestManager _requestManager;
        private readonly InputValidation _inputValidation;

        public RequestController(IRequestManager requestManager, InputValidation inputValidation)
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
                                result = await _requestManager.GetUserRequest(username);
                                if (result.IsSuccessful)
                                {
                                    return Ok(result.Payload!) ;
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

                return BadRequest("Not Authorized");
            }

            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("accept")]
        public async Task<IActionResult> AcceptRequest([FromBody] ServiceInfo service)
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
                                Result result = await _requestManager.AcceptServiceRequest(service.RequestId);
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
        public async Task<IActionResult> DeclineRequest([FromBody] ServiceInfo service)
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
                                Result result = await _requestManager.RemoveServiceRequest(service.RequestId, username);
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

        
        public class ServiceInfo
        {
            public string RequestId { get; set; } = string.Empty;
        }

    }
}

