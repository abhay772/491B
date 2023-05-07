using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DIYController : ControllerBase
    {
        private readonly IDIYManager _diyManager;
        private DIYObject _diyObject;
        public DIYController(IDIYManager diyManager, DIYObject dIYObject)
        {
            _diyObject = dIYObject;
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
        public async Task<IActionResult> UploadInfo(string email, string name, string description)
        {
            var result = await _diyManager.UploadInfoAsync(email, name, description);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("uploadvideo")]
        public async Task<IActionResult> UploadVideo(string email, string name, IFormFile videoFile)
        {
            var result = await _diyManager.UploadVideoAsync(email, name, videoFile);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("getDashboardDIY")]
        public IActionResult GetDashboardDIY(string email)
        {
            var result = _diyManager.GetDashboardDIY(email);
            //  result is a List<DIYObject>
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("searchDIY")]
        public IActionResult SearchDIY(string searchTerm)
        {
            var result = _diyManager.SearchDIY(searchTerm);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("getDIY")]
        public IActionResult GetDIY(string email, string name)
        {
            var result = _diyManager.GetDIY(email, name);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
        [HttpGet]
        [Route("getVideo")]
        public IActionResult GetVideo(DIYObject dIYObject)
        {
            MemoryStream videoStream = _diyManager.GetDIY(dIYObject.Email, dIYObject.Name).Video;
            if (videoStream == null)
            {
                return null;
            }
            return new FileStreamResult(videoStream, "video/mp4");
        }

        [HttpPost]
        [Route("addDIY")]
        public IActionResult AddDIY(string id, string email)
        {
            var result = _diyManager.AddDIY(id, email);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
