
using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers
{
    public interface IAuthManager
    {
        Task<Result> Login(string username, string password);
    }
}