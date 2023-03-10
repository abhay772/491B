using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.Interfaces
{
    public interface IAccountManager
    {
        Task<Result> RegisterUser(string email, string password, string firstname, string lastname, string role);

        Task<Result> RemoveUser(string email, string password);
    }
}
