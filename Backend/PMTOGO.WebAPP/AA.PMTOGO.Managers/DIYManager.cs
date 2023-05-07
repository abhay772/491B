using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Http;
using AA.PMTOGO.Managers.Interfaces;


namespace AA.PMTOGO.Managers
{
    public class DIYManager : IDIYManager
    {
        private readonly IDIYService _diyService;
        private readonly ILogger? _logger;
        private Result _result;
        public DIYManager(IDIYService diyService, ILogger logger, Result result)
        {
            _diyService = diyService;
            _logger = logger;
            _result = result;
        }
        public async Task<bool> UploadInfoAsync(string email, string name, string description)
        {
            var dao = new DIYDAO();
            var result = await dao.UploadInfo(email, name, description);
            return result;
        }

        public async Task<bool> UploadVideoAsync(string email, string name, IFormFile videoFile)
        {
            var result = await _diyService.UploadVideo(email, name, videoFile);
            return result;
        }

        public List<DIYObject> GetDashboardDIY(string email)
        {
            var dao = new DIYDAO();
            var result = dao.GetDashboardDIY(email);
            return result;
        }
        public List<DIYObject> SearchDIY(string searchTerm)
        {

            var result = _diyService.SearchDIY(searchTerm);
            return result;
        }
        public DIYObject GetDIY(string email, string name)
        {
            var dao = new DIYDAO();
            var result = dao.GetDIY(email, name).Result;
            return result;
        }

        public bool AddDIY(string id, string email)
        {

            var result = _diyService.AddDIY(id, email);
            return result;
        }
    }
}
