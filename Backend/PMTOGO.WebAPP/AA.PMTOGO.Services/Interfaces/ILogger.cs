using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Logging
{
    public interface ILogger
    {
        public Task<Result> Log(string requestName, byte logLevel, LogCategory logCategory, Result result);
    }
}
