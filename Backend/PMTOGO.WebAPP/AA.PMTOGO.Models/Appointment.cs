using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models
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
