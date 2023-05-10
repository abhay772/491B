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

        public UsageAnalysisManager(IUsageAnalysisDashboard usageDashboard)
        {
            _usageDashboard = usageDashboard;
        }

        public async Task<Result> GetAnalysis()
        {
            Result result = new Result();
            try
            {
                result = await _usageDashboard.GenerateAnalysis();
                return result;
            }
            catch
            {
                result.IsSuccessful= false;
                result.ErrorMessage = "Load Usage Analysis Dashboard Unsuccessful";
            }
            return result;

        }
    }
}
