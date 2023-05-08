using AA.PMTOGO.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DIYController : ControllerBase
    {
        private readonly IDIYManager _diyManager;
        public DIYController(IDIYManager diyManager)
        {
            _diyManager = diyManager;
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
        [Route("uploadinfo")]
        [ActionName("UploadInfo")]
        public async Task<IActionResult> UploadDIY([FromForm] DIY diy)
        {
            var result = await _diyManager.UploadInfoAsync(diy.email, diy.name, diy.description);

            var result2 = await _diyManager.UploadVideoAsync(diy.email, diy.name, diy.videofile);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("getDashboardDIY")]
        [ActionName("GetDashboardDIY")]
        public IActionResult GetDashboardDIY([FromBody] JsonElement data)
        {
            var username = data.GetProperty("username").GetString();
            var result = _diyManager.GetDashboardDIY(username);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        /*[HttpGet]
        [Route("searchDIY")]
        [ActionName("SearchDIY")]
        public IActionResult SearchDIY([FromBody] DIY diy)
        {
            var result = _diyManager.SearchDIY(diy.searchTerm);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }*/

        [HttpGet]
        [Route("getDIY")]
        [ActionName("GetDIY")]
        public IActionResult GetDIY([FromBody] DIY diy)
        {
            var result = _diyManager.GetDIY(diy.email, diy.name);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
        [HttpGet]
        [Route("getVideo")]
        [ActionName("GetVideo")]
        public IActionResult GetVideo([FromBody] DIY diy)
        {
            MemoryStream videoStream = _diyManager.GetDIY(diy.email, diy.name).Video!;
            if (videoStream == null)
            {
                return null!;
            }
            return new FileStreamResult(videoStream, "video/mp4");
        }


        public class DIY
        {
            public string email { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public IFormFile videofile { get; set; }
        }
    }
}
