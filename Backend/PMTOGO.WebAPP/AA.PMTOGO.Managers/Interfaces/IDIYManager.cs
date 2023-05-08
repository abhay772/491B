using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IDIYManager
    {
        Task<bool> UploadInfoAsync(string email, string name, string description);

        Task<bool> UploadVideoAsync(string email, string name, IFormFile videoFile);

        List<DIYDashboardObject> GetDashboardDIY(string email);

        List<DIYObject> SearchDIY(string searchTerm);

        DIYObject GetDIY(string email, string name);

        bool AddDIY(string id, string email);
    }
}
