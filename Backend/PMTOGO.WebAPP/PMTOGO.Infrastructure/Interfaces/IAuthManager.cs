using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IAuthManager
    {
        Task<Result> Login(string username, string password);
    }
}
