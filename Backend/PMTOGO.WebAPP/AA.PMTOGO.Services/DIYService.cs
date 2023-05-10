using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace AA.PMTOGO.Services
{
    public class DIYService : IDIYService
    {
        private readonly DIYDAO? _diyDao;
        public async Task<bool> UploadVideo(string email, string name, IFormFile videoFile)
        {
            // Convert IFormFile to byte[]
            byte[] videoBytes;
            using (var stream = videoFile.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    videoBytes = memoryStream.ToArray();
                }
            }

            return await _diyDao!.UploadVideo(email, name, videoBytes);
        }
        public List<DIYObject> SearchDIY(string name)
        {
            var dao = new DIYDAO();
            var result = dao.SearchDIY(name);
            return result;
        }

        public bool AddDIY(string id, string email)
        {
            var dao = new DIYDAO();

            var result = dao.AddDIY(id, email).Result;

            return result;
        }

    }
}
