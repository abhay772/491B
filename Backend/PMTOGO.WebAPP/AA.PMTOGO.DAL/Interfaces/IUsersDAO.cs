using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL.Interfaces
{
    public interface IUsersDAO
    {
        public Task<Result> FindUser(string username);
    
        public Task<Result> GetUser(string username);
        public Task<Result> DoesUserExist(string email);

        public Task<Result> DeleteUserAccount(string username);

        public Task<Result> DeleteUserProfile(string username);

        public Task<Result> ActivateUser(string username);
        //non-sensitive info
        public Task<Result> SaveUserProfile(string email, string firstName, string lastName, string role);
        public Task<Result> SaveUserAccount(string username, string passDigest, string salt, string role);

        public Task UpdateFailedAttempts(string username);

        public Task<int> GetFailedAttempts(string username);

        public Task ResetFailedAttempts(string username);

        public Task<Result> RequestRecovery(string username);

        public Task<Result> ValidateOTP(string username, string otp);

        public Task<Result> UpdatePassword(string username, string passDigest, string salt);

        public Task<Result> SaveOTP(string username, string otp);
    }
}
