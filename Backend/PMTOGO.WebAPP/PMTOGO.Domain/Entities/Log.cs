using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AA.PMTOGO.Models.Entities
{
    public enum LogCategory
    {
        View,
        Business,
        Server,
        Data,
        DataStore,
    }

    [Table("Logs")]
    public class Log
    {
        [Key]
        [Column("logId")]
        public Guid LogId { get; } = Guid.NewGuid();
        [Column("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        //LogLevel is type byte due to its limited enumerations.
        //A byte compared to int would shave computation time for efficiency.
        [Column("logLevel")]
        public byte LogLevel { get; set; }        
        [Column("operation")]
        public string Operation { get; set; } = string.Empty;
        [Column("logCategory")]
        public string Category { get; set; } = string.Empty;
        [Column("message")]
        public string Message { get; set; } = string.Empty;
    }
    
}
