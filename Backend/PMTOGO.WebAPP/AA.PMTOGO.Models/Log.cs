using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models
{
    public enum LogCategory
    {
        View,
        Business,
        Server,
        Data,
        DataStore,
    }
    public class Log
    {
        public Guid LogId { get; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        //LogLevel is type byte due to its limited enumerations.
        //A byte compared to int would save computation time for efficiency.
        public byte LogLevel { get; set; }
        public string Operation { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
