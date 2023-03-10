using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.Interfaces
{
    public interface IAuthenticator
    {
        Result Authenticate(string username, string password);
        string EncryptPassword(string password, string salt);
        string GenerateOTP();
        Task<int> GetFailedAttempts(string username);
        void ResetFailedAttempts(string username);

    }
}
