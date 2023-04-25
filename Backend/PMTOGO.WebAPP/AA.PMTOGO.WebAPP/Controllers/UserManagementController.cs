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
    public class UserManagementController : ControllerBase
    {
        
        private readonly IAccountManager _accManager;
        private readonly ClaimValidation _claims;
            

        public UserManagementController(IAccountManager accManager, ClaimValidation claims)
        {
            _accManager = accManager;
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

        [HttpPost]
        [Route("register")]
        //[Consumes("application/json")]
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

        [HttpPost]
        [Route("recovery")]
        [ActionName("AccountRecovery")]
        public async Task<ActionResult> AccountRecovery([FromBody] UserRegister user)
        {
            try
            {
                Result result = await _accManager.RecoverAccount(user.Email);
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
        [Route("otp")]
        [ActionName("ValidateOTP")]
        public async Task<ActionResult> ValidateOTP([FromBody] UserRegister user)
        {
            try
            {
                Result result = await _accManager.OTPValidation(user.Email, user.OTP);
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
        [Route("updatePassword")]
        [ActionName("UpdatePassword")]
        public async Task<ActionResult> UpdatePassword([FromBody] UserRegister user)
        {
            try
            {
                Result result = await _accManager.UpdatePassword(user.Email, user.Password);
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

        [HttpDelete]
        [Route("delete")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteUser()
        {
            Result result = new Result();
            result = _claims.ClaimsValidation("Service Provider", Request);
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {

                try
                {
                    Result delete = await _accManager.DeleteUserAccount(user.ClaimUsername);
                    if (delete.IsSuccessful)
                    {
                        return Ok(delete);
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

        public class UserRegister
        {
            public string Email { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string OTP { get; set; } = string.Empty;
        }
    }
}
