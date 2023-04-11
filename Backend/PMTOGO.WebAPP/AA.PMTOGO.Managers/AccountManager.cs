using AA.PMTOGO.Logging;
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

        public AccountManager(IUserManagement account, ILogger logger)
        {
            _account = account;
            _logger = logger;
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

        public async Task<Result> DeleteUserAccount(string username)
        {
            Result result = await _account.DeleteAccount(username);
            return result;
        }
    }
}
