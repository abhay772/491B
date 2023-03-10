using PMTOGO.WebAPP.Interfaces;
using PMTOGO.WebAPP.Models.Entities;
using System.Diagnostics;
using ILogger = PMTOGO.WebAPP.Interfaces.ILogger;


//all request regarding the users account will be sent the the Account manager
namespace PMTOGO.WebAPP.Managers
{
    public class AccountManager: IAccountManager
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
            Result result = await Task.Run(() => _account.CreateAccount(email, password, firstname, lastname, role));
            timer.Stop();
            var seconds = timer.ElapsedMilliseconds / 1000;
            if (seconds > 5)
            {
                Result resultLog = new Result();
                resultLog.ErrorMessage = "Took" + seconds + "seconds to create user, longer than alloted ";
                _logger!.Log("RegisterUser", 1, LogCategory.Data, resultLog);
                //log it took longer than 5 seconds 
            }
                
            return result;
        }

        public async Task<Result> RemoveUser(string username, string password)
        {
            Result result = await Task.Run(() => _account.DeactivateAccount(username, password));
            return result;
        }
    }
}
