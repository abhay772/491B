using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IUsageAnalysisManager
    {
        Task<Result> GetAnalysis();
    }
}
