﻿using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface IUserManagement
    {
        Task<User?> GetUser(string username);

        Task<Result> GatherUsers();
        Task<Result> CreateAccount(string email, string password, string firstname, string lastname, string role);
        Task<Result> DeleteAccount(string username);

        Task<Result> EnableAccount(string username, int active);

        Task<Result> DisableAccount(string username, int active);
        string GenerateSalt();
        string EncryptPassword(string password, string salt);
        Task<Result> AccountRecovery(string email);
    }
}
