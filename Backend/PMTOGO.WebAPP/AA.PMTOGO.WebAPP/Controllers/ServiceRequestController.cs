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
        private readonly ClaimValidation _claims;

        public ServiceRequestController(IServiceRequestManager requestManager,ClaimValidation claims)
        {
            _requestManager = requestManager;
            _claims = claims; //uses input validation

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
            result = _claims.ClaimsValidation("Service Provider", Request);
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
            result = _claims.ClaimsValidation("Service Provider", Request);

            if (result.IsSuccessful)
            {
                try
                {
                    Result accept = await _requestManager.AcceptServiceRequest(service.Id);
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
            result = _claims.ClaimsValidation("Service Provider", Request);
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {

                try
                {
                    Result removal = await _requestManager.RemoveServiceRequest(service.Id, user.ClaimUsername);
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

    }
}

