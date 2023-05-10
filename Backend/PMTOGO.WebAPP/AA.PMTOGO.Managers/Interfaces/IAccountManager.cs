using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<Result> RegisterUser(string email, string password, string firstname, string lastname, string role);

        Task<Result> RecoverAccount(string username);
        Task<Result> DisableUserAccount(string username);
        Task<Result> EnableUserAccount(string username);
        Task<Result> DeleteUserAccount(string email);
        Task<Result> UpdatePassword(string username, string password);
        Task<Result> OTPValidation(string username, string otp);

        Task<Result> GetAllUsers();
    }
}
