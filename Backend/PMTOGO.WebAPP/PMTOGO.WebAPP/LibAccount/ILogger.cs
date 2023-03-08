using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.LibAccount
{
    public interface ILogger
    {
        public void Log(string requestName, byte logLevel, LogCategory logCategory, object result);
    }
}
