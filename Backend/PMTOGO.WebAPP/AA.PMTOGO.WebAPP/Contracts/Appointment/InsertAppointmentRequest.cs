
namespace AA.PMTOGO.WebAPP.Contracts.Appointment;

public record InsertAppointmentRequest()
{
    public int UserId;
    public string? Title;
    public DateTime AppointmentTime;
};