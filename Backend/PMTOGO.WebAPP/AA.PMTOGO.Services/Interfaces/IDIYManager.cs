using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Http;


namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IDIYManager
    {
        Task<bool> UploadInfoAsync(string email, string name, string description);


        Task<bool> UploadVideoAsync(string email, string name, IFormFile videoFile);


        List<DIYObject> GetDashboardDIY(string email);

        List<DIYObject> SearchDIY(string searchTerm);

        DIYObject GetDIY(string email, string name);

        bool AddDIY(string id, string email);
    }
}
