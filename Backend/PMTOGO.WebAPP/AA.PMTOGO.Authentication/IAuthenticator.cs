using AA.PMTOGO.Models;

namespace AA.PMTOGO.Authentication
{
    public interface IAuthenticator
    {
        Result Authenticate(string username, string password);
        bool CheckValidOTP(string otp);
        byte[] EncrpytPassword(string password, byte[] salt);
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