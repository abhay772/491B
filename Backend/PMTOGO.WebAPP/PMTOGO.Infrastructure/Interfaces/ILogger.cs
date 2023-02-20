using PMTOGO.Domain.Entities;

namespace PMTOGO.Infrastructure.Interfaces
{
    public interface ILogger
    {
        public void Log(string requestName, byte logLevel, LogCategory logCategory, object result);
    }
}
