using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IDIYService
    {
        Task<bool> UploadVideo(string email, string name, IFormFile videoFile);

        List<DIYObject> SearchDIY();

        bool AddDIY(string id, string email);

    }
}
