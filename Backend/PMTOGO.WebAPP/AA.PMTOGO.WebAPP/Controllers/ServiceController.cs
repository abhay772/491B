
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
        private readonly ClaimValidation _claims;

        public ServiceController(IServiceManager serviceManager, ClaimValidation claims)
        {
            _serviceManager = serviceManager;
            _claims = claims;//uses input validation
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
            result = _claims.ClaimsValidation(null!, Request);
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
            result = _claims.ClaimsValidation(null!, Request);

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
        public async Task<IActionResult> AddServiceRequest(ServiceRequest service)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation("Property Manager", Request);
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {
                try
                {
                    Result insert = await _serviceManager.AddServiceRequest(service, user.ClaimUsername);
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
        [Route("rate")]
        public async Task<IActionResult> RateService(ServiceInfo service)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation("Property Manager", Request);

            if (result.IsSuccessful )
            {
                try
                {
                    Result rating = await _serviceManager.RateUserService(service.Id, service.rate);
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

    }
}
