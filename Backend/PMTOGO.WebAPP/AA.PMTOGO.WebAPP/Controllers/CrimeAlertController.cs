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
    public class CrimeAlertController : ControllerBase
    {
        private readonly ICrimeMapManager _mapManager;
        private readonly ClaimValidation _claims;
        public CrimeAlertController(ICrimeMapManager mapManager, ClaimValidation claims)
        {
            _mapManager = mapManager;
            _claims = claims;
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
        [Route("addAlert")]
        [ActionName("AddCrimeAlert")]
        public async Task<IActionResult> AddCrimeAlert([FromBody] Alert alert)
        {
            try
            {
                Result result = await _mapManager.AddCrimeAlert(alert.Email, alert.Name, alert.Location, alert.Description, alert.Time, alert.Date, alert.X, alert.Y);
                if (result.IsSuccessful)
                {
                    return Ok(new { message = "Crime alert added successfully." });
                }
                else
                {

                    return BadRequest("Error");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("viewAlert")]
        [ActionName("ViewCrimeAlert")]
        public async Task<IActionResult> ViewCrimeAlert([FromBody] Alert alert)
        {
            try
            {
                var crimeAlert = await _mapManager.ViewCrimeAlert(alert.Email, alert.ID);
                if (crimeAlert is not null)
                {
                    return Ok(new { message = "Crime alert added successfully." });
                }
                else
                {

                    return BadRequest("Error");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("editAlert")]
        [ActionName("EditCrimeAlert")]
        public async Task<IActionResult> EditCrimeAlert([FromBody] Alert alert)
        {
            try
            {
                Result result = await _mapManager.EditCrimeAlert(alert.Email, alert.ID, alert.Name, alert.Location, alert.Description, alert.Time, alert.Date, alert.X, alert.Y);
                if (result.IsSuccessful)
                {
                    return Ok(new { message = "Crime alert added successfully." });
                }
                else
                {

                    return BadRequest("Error");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("deleteAlert")]
        [ActionName("DeleteCrimeAlert")]
        public async Task<IActionResult> DeleteCrimeAlert([FromBody] Alert alert)
        {
            try
            {
                Result result = await _mapManager.DeleteCrimeAlert(alert.Email, alert.ID);
                if (result.IsSuccessful)
                {
                    return Ok(new { message = "Crime alert added successfully." });
                }
                else
                {

                    return BadRequest("Error");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("getAlerts")]
        [ActionName("GetCrimeAlert")]
        public async Task<IActionResult> GetCrimeAlert()
        {
            try
            {
                var crimeAlerts = await _mapManager.GetCrimeAlerts();
                if (crimeAlerts is not null)
                {
                    return Ok(crimeAlerts);
                }
                else
                {
                    return BadRequest("Error");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public class Alert
        {
            public string Email { get; set; } = string.Empty;
            public int ID { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Time { get; set; } = string.Empty;
            public string Date { get; set; } = string.Empty;
            public float X { get; set; }
            public float Y { get; set; }
        }
    }
}
