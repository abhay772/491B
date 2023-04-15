using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Infrastructure.Interfaces;
﻿using AA.PMTOGO.Logging;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System.Diagnostics;

namespace AA.PMTOGO.Managers
{
    //input validation, error handling , logging
    public class AccountManager : IAccountManager
    {
        private readonly IUserManagement _account;
        private readonly ILogger? _logger;
        private readonly IUsersDAO _usersDAO;

        public AccountManager(IUserManagement account, ILogger logger, IUsersDAO usersDAO)
        {
            _account = account;
            _logger = logger;
            _usersDAO = usersDAO;
        }

        public async Task<Result> RegisterUser(string email, string password, string firstname, string lastname, string role)
        {
            var timer = Stopwatch.StartNew();
            Result result = await _account.CreateAccount(email, password, firstname, lastname, role);
            timer.Stop();
            var seconds = timer.ElapsedMilliseconds / 1000;
            if (seconds > 5)
            {
                Result resultLog = new Result();
                resultLog.ErrorMessage = "Took" + seconds + "seconds to create user, longer than alloted ";
                await _logger!.Log("RegisterUser", 1, LogCategory.Data, resultLog);
                //log it took longer than 5 seconds 
            }

            return result;
        }

        public async Task<Result> RecoverAccount(string username)
        {
            Result result = await _account.AccountRecovery(username);
            return result;
        }

        public async Task<Result> DeleteUserAccount(string email)
        {
            Result result = await _account.DeleteAccount(email);
            return result;
        }

        public async Task<Result> OTPValidation(string username, string otp)
        {
            Result result = await _usersDAO.ValidateOTP(username, otp);
            return result;
        }
        public async Task<Result> UpdatePassword(string username, string password)
        {
            string salt = _account.GenerateSalt();
            string passDigest = _account.EncryptPassword(password, salt);
            Result result = await _usersDAO.UpdatePassword(username, passDigest, salt);
            return result;
        }
    }
}
