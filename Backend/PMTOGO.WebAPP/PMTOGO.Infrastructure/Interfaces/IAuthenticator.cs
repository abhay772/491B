using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Authentication
{
    public interface IAuthenticator
    {
        Task<Result> Authenticate(string username, string password);
        bool CheckValidOTP(string otp);
        string GenerateOTP();
        Task<int> GetFailedAttempts(string username);
        void ResetFailedAttempts(string username);
        void UpdateFailedAttempts(string username);
        Result ValidateDateOfBirth(DateTime dob);
        Result ValidateEmail(string email);
        Result ValidatePassphrase(string passWord);
        Result ValidateUsername(string name);
    }
}