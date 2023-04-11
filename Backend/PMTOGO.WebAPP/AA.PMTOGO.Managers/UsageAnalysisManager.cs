﻿using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
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
        UsageAnalysisDashboard usageDashboard = new UsageAnalysisDashboard();
        public async Task<Result> GetAnalysis()
        {
            Result result = await usageDashboard.GenerateAnalysis();
            return result;

        }
    }
}
