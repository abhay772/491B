using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;


namespace AA.PMTOGO.DAL
{
    public interface ILoggerDAO
    {

        public Task<Result> InsertLog(Log log);

        public Task<Result> GetAnalysisLogs(string operation);
    }
}
