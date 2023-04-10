using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IUsageAnalysisManager
    {
        Task<Result> GetAnalysis();
    }
}
