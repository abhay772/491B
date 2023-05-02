using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrimeAlertController : ControllerBase
    {
        private readonly ICrimeMapManager _mapManager;
        public CrimeAlertController(ICrimeMapManager mapManager)
        {
            _mapManager = mapManager;
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

                    return BadRequest("Error: Unable to add alert.");
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
                    return Ok(new { message = "Crime alert edited successfully." });
                }
                else
                {

                    return BadRequest("Error: Unable to edit alert.");
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
                    return Ok(new { message = "Crime alert deleted successfully." });
                }
                else
                {

                    return BadRequest("Error: Unable to delete.");
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
            [Required]
            [EmailAddress]
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
