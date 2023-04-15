
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AA.PMTOGO.Models.Entities
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }
        public string Username { get; set; }
        public string? Title { get; set; } = null!;
        public DateTime AppointmentTime { get; set; }

        public virtual User? User { get; set; }
    
        //mapping for InsertAppointmentRequest
        public Appointment(string title, DateTime appointmentTime)
        {   
            this.AppointmentId = 0;
            this.Title = title;
            this.AppointmentTime = AppointmentTime;
        }

        //mapping for UpdateAppointmentRequest
        public Appointment(int appointmentId, string username, string title, DateTime appointmentTime)
        {
            this.AppointmentId = appointmentId;
            this.Username = username;
            this.Title = title;
            this.AppointmentTime = appointmentTime;
        }
    }
}
