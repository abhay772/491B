using AA.PMTOGO.Models;

namespace AA.PMTOGO.Managers
{
    public interface IAuthManager
    {
        Task<Result> Login(string username, string password);
    }
}