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
        private readonly ClaimValidation _claims;

        public AdminController(IUsageAnalysisManager analysisManager, ClaimValidation claims)
        {
            _analysisManager = analysisManager;
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
        [Route("getusageanalysis")]
        [Consumes("application/json", "application/problem+json")]
        public async Task<IActionResult> GetUsageAnalysis()
        {
            Result result = new Result();
            result = _claims.ClaimsValidation("Admin", Request);

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
        

    }
}
