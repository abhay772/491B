using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Infrastructure.Interfaces;
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
        private readonly IUsersDAO _usersDAO;

        public AccountManager(IUserManagement account, ILogger logger, IUsersDAO usersDAO)
        {
            _account = account;
            _logger = logger;
            _usersDAO = usersDAO;
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

        public async Task<Result> DisableUserAccount(string username)
        {
            Result result = new Result();
            try
            {
                result = await _account.DisableAccount(username, false);
                await _logger!.Log("DisableUserAccount", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Disable Account Unsuccessful. Try Again Later";
                await _logger!.Log("DisableUserAccount", 4, LogCategory.Business, result);
            }
            return result;
        }

        public async Task<Result> EnableUserAccount(string username)
        {
            Result result = new Result();
            try
            {
                result = await _account.EnableAccount(username, true);
                await _logger!.Log("EnableUserAccount", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Enable Account Unsuccessful. Try Again Later";
                await _logger!.Log("EnableUserAccount", 4, LogCategory.Business, result);
            }
            return result;
        }
    }
}
