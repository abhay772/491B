using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IDIYService
    {
        Task<bool> UploadVideo(string email, string name, IFormFile videoFile);
        
        List<DIYObject> SearchDIY(string name);

        bool AddDIY(string id, string email);
      
    }
}
