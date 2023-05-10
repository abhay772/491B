using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface IUsageAnalysisDashboard
    {
        // gathers logins and registrations stats
        Task<Result> GenerateAnalysis();

        Task<Result> GetLoginsPerDay();

        Task<Result> GetRegistrationsPerDay();
    }
}
