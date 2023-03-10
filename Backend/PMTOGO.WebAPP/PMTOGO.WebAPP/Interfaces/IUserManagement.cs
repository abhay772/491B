using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.Interfaces
{
    public interface IUserManagement
    {
        Result CreateAccount(string email, string password, string firstname, string lastname, string role);
        Result DeactivateAccount(string username, string password);
        string GenerateSalt();
        string EncryptPassword(string password, string salt);
    }
}
