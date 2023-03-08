using Microsoft.AspNetCore.Mvc;
using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.Managers
{
    public interface IAuthManager
    {
        Task<Result> Login(string username, string password);

        Task<Result> RegisterUser(string email, string password, string firstname, string lastname, string role);
    }
}
