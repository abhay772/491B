using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.LibAccount
{
    public interface IAuthenticator
    {
        Result Authenticate(string username, string password);
        Result CreateUser(string email, string password,
            string firstname, string lastname, string role);
        bool CheckValidOTP(string otp);
        string EncryptPassword(string password, string salt);
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
