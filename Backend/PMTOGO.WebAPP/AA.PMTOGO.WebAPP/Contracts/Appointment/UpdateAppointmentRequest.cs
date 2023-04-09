
namespace AA.PMTOGO.WebAPP.Contracts.Appointment;

public record UpdateAppointmentRequest()
{
    public int UserId;
    public string? Title;
    public DateTime AppointmentTime;
};