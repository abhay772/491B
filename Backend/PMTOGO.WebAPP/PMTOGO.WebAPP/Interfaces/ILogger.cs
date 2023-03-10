using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.Interfaces
{
    public interface ILogger
    {
        public void Log(string requestName, byte logLevel, LogCategory logCategory, object result);
    }
}
