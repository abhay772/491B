using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IAuthManager
    {
        Task<Result> Login(string username, string password);
    }
}
