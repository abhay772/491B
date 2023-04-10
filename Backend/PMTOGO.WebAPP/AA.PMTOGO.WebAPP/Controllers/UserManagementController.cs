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
        private readonly InputValidation _inputValidation;

        public UserManagementController(IAccountManager accManager, InputValidation inputValidation)
        {
            _accManager = accManager;
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
        //[Consumes("application/json")]
        [ActionName("AccountRecovery")]
        public async Task<IActionResult> AccountRecovery([FromBody] string email)
        {
            try
            {
                Result result = await _accManager.RecoverAccount(email);
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
        //[Consumes("application/json")]
        [ActionName("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP([FromBody] string otp)
        {
            try
            {
                Result result = await _accManager.OTPValidation(otp);
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
        //[Consumes("application/json")]
        [ActionName("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] string password)
        {
            try
            {
                Result result = await _accManager.UpdatePassword(password);
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

                        if (role != null && validationCheck)
                        {

                            try
                            {
                                Result result = await _accManager.DeleteUserAccount(username);
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
                    }

                }

                return BadRequest("Cookie not found");
            }

            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
