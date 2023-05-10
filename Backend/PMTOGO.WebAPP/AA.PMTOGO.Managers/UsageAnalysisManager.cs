using AA.PMTOGO.Logging;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using AA.PMTOGO.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Managers
{
    //input validation, error handling , logging
    public class UsageAnalysisManager : IUsageAnalysisManager
    {
        private readonly IUsageAnalysisDashboard _usageDashboard;
        private readonly ILogger? _logger;

        public UsageAnalysisManager(IUsageAnalysisDashboard usageDashboard, ILogger logger)
        {
            _usageDashboard = usageDashboard;
            _logger = logger;
        }

        public async Task<Result> GetAnalysis()
        {
            Result result = new Result();
            try
            {
                result = await _usageDashboard.GenerateAnalysis();
                await _logger!.Log("GetAnalysis", 4, LogCategory.Business, result); 
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load Usage Analysis Dashboard Unsuccessful";
                await _logger!.Log("GetAnalysis", 4, LogCategory.Business, result);
            }
            return result;

        }
    }
}