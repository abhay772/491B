using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
