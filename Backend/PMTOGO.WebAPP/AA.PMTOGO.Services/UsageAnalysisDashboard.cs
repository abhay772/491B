using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Services
{
    public class UsageAnalysisDashboard : IUsageAnalysisDashboard
    {
        Logger _logger = new();
        public Task<Result> GenerateAnalysis()
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetLoginsPerDay()
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetRegistrationsPerDay()
        {
            throw new NotImplementedException();
        }
    }
}
