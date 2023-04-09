
using System.ComponentModel.DataAnnotations;

namespace AA.PMTOGO.Models.Entities
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; } = null!;
        public DateTime AppointmentTime { get; set; }
    }
}
