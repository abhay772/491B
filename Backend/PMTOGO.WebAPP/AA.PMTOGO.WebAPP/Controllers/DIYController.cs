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
        [Route("uploadDIY")]
        [ActionName("UploadInfo")]
        public async Task<IActionResult> UploadDIY([FromForm] DIY diy)
        {
            var result = await _diyManager.UploadInfoAsync(diy.email, diy.name, diy.description);

            var result2 = await _diyManager.UploadVideoAsync(diy.email, diy.name, diy.videofile);

            var getdiy = _diyManager.GetDIY(diy.email, diy.name);

            var result3 = _diyManager.AddDIY(getdiy.ID, diy.email);


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
            var result = _diyManager.GetDashboardDIY(username!);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("searchDIY")]
        [ActionName("SearchDIY")]
        public IActionResult SearchDIY()
        {
            var result = _diyManager.SearchDIY();


            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

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
        [HttpPost]
        [Route("getVideo")]
        [ActionName("GetVideo")]
        public IActionResult GetVideo([FromBody] DIYDashboard diy)
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
            public string email { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public IFormFile videofile { get; set; }
        }
        public class DIYDashboard
        {
            public string id { get; set; } = string.Empty;
            public string email { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
        }
    }
}
