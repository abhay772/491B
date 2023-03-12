using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ILogger = AA.PMTOGO.Infrastructure.Interfaces.ILogger;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        
        private readonly IAccountManager _accManager;
        private readonly ILogger _logger;

        public UserManagementController(IAccountManager accManager, ILogger logger)
        {
            _accManager = accManager;
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

        [HttpPost]
        [Route("register")]
        //[Route("{email, password, firstName, lastName, role}")]
        [ActionName("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserRegister user)
        {
            try
            {
                Result result = await _accManager.RegisterUser(user.Email, user.Password, user.FirstName, user.LastName, user.Role);
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
        [Route("deactivate")]
        public async Task<IActionResult> DeleteUser([FromBody] UserCredentials userCredentials)
        {
            try
            {
                Result result = await _accManager.RemoveUser(userCredentials.Username, userCredentials.Password);
                if (result.IsSuccessful)
                {
                    return Ok(result);
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

        public class UserCredentials
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
        public class UserRegister
        {
            public string Email { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;

        }
    }
}
