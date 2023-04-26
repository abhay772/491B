using AA.PMTOGO.DAL;
﻿using AA.PMTOGO.Logging;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System.Data;
using System.Diagnostics;

namespace AA.PMTOGO.Managers
{
    //input validation, error handling , logging
    public class AccountManager : IAccountManager
    {
        private readonly IUserManagement _account;
        private readonly ILogger? _logger;

        public AccountManager(IUserManagement account, ILogger logger)
        {
            _account = account;
            _logger = logger;
        }

        public async Task<Result> RegisterUser(string email, string password, string firstname, string lastname, string role)
        {
            Result result = new Result();
            try
            {
                var timer = Stopwatch.StartNew();
                result = await _account.CreateAccount(email, password, firstname, lastname, role);
                timer.Stop();
                var seconds = timer.ElapsedMilliseconds / 1000;
                if (seconds > 5)
                {
                    Result resultLog = new Result();
                    resultLog.ErrorMessage = "Took" + seconds + "seconds to create user, longer than alloted ";
                    await _logger!.Log("RegisterUser", 1, LogCategory.Data, resultLog);
                    //log it took longer than 5 seconds 
                }
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Delete Account Unsuccessful. Try Again Later";
                await _logger!.Log("RegisterUser", 4, LogCategory.Business, result);

            }
            return result;
        }

        public async Task<Result> RecoverAccount(string username)
        {
            Result result = new Result();
            try
            {
                result = await _account.AccountRecovery(username);
                await _logger!.Log("AccountRecovery", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Account Recovery Unsuccessful. Try Again Later";
                await _logger!.Log("AccountRecovery", 4, LogCategory.Business, result);
            }
            return result;
        }

        public async Task<Result> DeleteUserAccount(string username)
        {
            Result result = new Result();
            try
            {
                result = await _account.DeleteAccount(username);
                await _logger!.Log("DeleteUserAccount", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Delete Account Unsuccessful. Try Again Later";
                await _logger!.Log("DeleteUserAccount", 4, LogCategory.Business, result);
            }
            return result;
        }

        public async Task<Result> OTPValidation(string username, string otp)
        {
            var dao = new UsersDAO();
            Result result = await dao.ValidateOTP(username, otp);
            return result;
        }
        public async Task<Result> UpdatePassword(string username, string password)
        {
            var dao = new UsersDAO();
            string salt = _account.GenerateSalt();
            string passDigest = _account.EncryptPassword(password, salt);
            Result result = await dao.UpdatePassword(username, passDigest, salt);
            return result;
        }
    }
}
