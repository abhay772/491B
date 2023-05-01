using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface IUserManagement
    {
        Task<Result> CreateAccount(string email, string password, string firstname, string lastname, string role);
        Task<Result> DeleteAccount(string username);
        string GenerateSalt();
        string EncryptPassword(string password, string salt);
        Task<Result> AccountRecovery(string email);
    }
}
