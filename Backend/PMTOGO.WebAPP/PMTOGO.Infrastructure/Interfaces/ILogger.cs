using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface ILogger
    {
        public Task Log(string requestName, byte logLevel, LogCategory logCategory, object result);
    }
}
