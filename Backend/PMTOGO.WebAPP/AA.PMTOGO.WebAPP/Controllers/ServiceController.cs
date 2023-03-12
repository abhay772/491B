using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ILogger = AA.PMTOGO.Infrastructure.Interfaces.ILogger;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController: ControllerBase
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
        public async Task<List<ServiceRequest>> GetRequest([FromBody] string username)
        {
            Result result = new Result();
            try
            {
                result = await _serviceManager.GetUserRequest(username);
                if (result.IsSuccessful)
                {
                    return (List<ServiceRequest>)result.Payload!;
                }
                else
                {
                    //return BadRequest("Invalid username or password provided. Retry again or contact system admin.");
                    return (List<ServiceRequest>)result.Payload!;
                }
            }
            catch
            {
                return (List<ServiceRequest>)result.Payload!;
                //return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



    }
}
