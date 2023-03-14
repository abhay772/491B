using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ILogger = AA.PMTOGO.Infrastructure.Interfaces.ILogger;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        //private readonly UsersDbContext usersDbContext;
        private readonly IServiceManager _serviceManager;
        private readonly ILogger _logger;

        public ServiceController(IServiceManager serviceManager, ILogger logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
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
        [Route("{email}")]
        public async Task<IActionResult> GetRequest([FromBody] string username)
        {
            Result result = new Result();
            try
            {
                result = await _serviceManager.GetUserRequest(username);
                if (result.IsSuccessful)
                {
                    return Ok(result.Payload!);
                }
                else
                {
                    return BadRequest("Invalid username or password provided. Retry again or contact system admin.");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("requests")]
        public async Task<IActionResult> AddServiceRequest(ServiceRequest serviceRequest)
        {
            try
            {
                Result result = await _serviceManager.RequestAService(serviceRequest);
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
        [HttpPost]
        [Route("accept")]
        public async Task<IActionResult> AcceptRequest([FromBody] ServiceRequest serviceRequest)
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
        [HttpPost]
        [Route("decline")]
        public async Task<IActionResult> DeclineRequest([FromBody] ServiceRequest serviceRequest)
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
        [HttpPut]
        [Route("{rate}")]
        public async Task<IActionResult> RateService([FromBody] UserService service, int rate)
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

        [HttpPost]
        [Route("service")]
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
