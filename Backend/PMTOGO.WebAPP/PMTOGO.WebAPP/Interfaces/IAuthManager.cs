using Microsoft.AspNetCore.Mvc;
using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.Interfaces
{
    public interface IAuthManager
    {
        Task<Result> Login(string username, string password);

       
    }
}
