using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Services
{
    //input validation, error handling , logging
    public class UsageAnalysisDashboard : IUsageAnalysisDashboard
    {
        private readonly ILoggerDAO _logger;

        public UsageAnalysisDashboard(ILoggerDAO loggerDAO)
        {
            _logger = loggerDAO;
        }

        public async Task<Result> GenerateAnalysis()
        {
            Result analysis = new Result();
            Result Logins = await GetLoginsPerDay();
            Result Registrations = await GetRegistrationsPerDay();
            return analysis;
        }

        public async Task<Result> GetLoginsPerDay()
        {
            Result logins = await _logger.GetAnalysisLogs("Authenticate");

            return logins;
        }

        public async Task<Result> GetRegistrationsPerDay()
        {
            Result registrations = await _logger.GetAnalysisLogs("CreateAccount");

            return registrations;
        }
    }
}
