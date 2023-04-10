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
    public class AdminController: ControllerBase
    {
        private readonly IUsageAnalysisManager _analysisManager;
        private readonly InputValidation _inputValidation;

        public AdminController(IUsageAnalysisManager analysisManager, InputValidation inputValidation)
        {
            _analysisManager = analysisManager;
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
        [Route("getusageanalysis")]
        [Consumes("application/json", "application/problem+json")]
        public async Task<IActionResult> GetUsageAnalysis()
        {
            Result result = new Result();
            result = ClaimsValidation("Admin");
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {
                try
                {
                    Result analysis = await _analysisManager.GetAnalysis();
                    if (analysis.IsSuccessful)
                    {
                        return Ok(analysis.Payload!);
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

    }
}
