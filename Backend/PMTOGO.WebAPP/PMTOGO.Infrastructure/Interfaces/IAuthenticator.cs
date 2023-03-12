using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IAuthenticator
    {
        Task<Result> Authenticate(string username, string password);
        string EncryptPassword(string password, string salt);
        string GenerateOTP();
        Task<int> GetFailedAttempts(string username);
        void ResetFailedAttempts(string username);

    }
}