
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
    public class UserServiceController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ClaimValidation _claims;

        public UserServiceController(IServiceManager serviceManager, ClaimValidation claims)
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
                    Result userServices = await _serviceManager.GetAllUserServices(user.ClaimUsername, user.ClaimRole);
                    if (userServices.IsSuccessful)
                    {
                        return Ok(userServices.Payload!);
                    }
                    else
                    {
                        return BadRequest(userServices.ErrorMessage);
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
                    //get all services service providers provide
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
        public async Task<IActionResult> ServiceRequest(ServiceInfo service)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation("Property Manager", Request);
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {
                try
                {
                    Result insert = await _serviceManager.AddServiceRequest(service.Id, service.frequency, service.comments, user.ClaimUsername);
                    if (insert.IsSuccessful)
                    {
                        return Ok(insert.Payload);
                    }
                    else
                    {

                        return BadRequest(insert.ErrorMessage);
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
            result = _claims.ClaimsValidation(null!, Request);
            UserClaims user = (UserClaims)result.Payload!;


            if (result.IsSuccessful)
            {
                try
                {//NEED role to correspond with service provider or property manager rating column
                    Result rating = await _serviceManager.RateUserService(service.Id, service.rate, user.ClaimRole);
                    if (rating.IsSuccessful)
                    {
                        return Ok(rating.Payload);
                    }
                    else
                    {

                        return BadRequest(result.ErrorMessage);
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
        [Route("frequencyrequest")]
        public async Task<IActionResult> ChangeServiceFrequncy(ServiceInfo service)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation("Property Manager", Request);


            if (result.IsSuccessful)
            {
                try
                {
                    Result frequency = await _serviceManager.FrequencyChangeRequest(service.Id, service.frequency);
                    if (frequency.IsSuccessful)
                    {
                        return Ok(frequency.Payload);
                    }
                    else
                    {

                        return BadRequest(result.ErrorMessage);
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
        [Route("cancelrequest")]
        public async Task<IActionResult> CancelService(ServiceInfo service)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation("Property Manager", Request);



            if (result.IsSuccessful)
            {
                try
                {
                    Result cancel = await _serviceManager.CancelRequest(service.Id);
                    if (cancel.IsSuccessful)
                    {
                        return Ok(cancel.Payload);
                    }
                    else
                    {

                        return BadRequest(result.ErrorMessage);
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