using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface ILogger
    {
        public void Log(string requestName, byte logLevel, LogCategory logCategory, object result);
    }
}
