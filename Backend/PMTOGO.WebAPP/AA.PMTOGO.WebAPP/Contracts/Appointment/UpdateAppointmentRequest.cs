
namespace AA.PMTOGO.WebAPP.Contracts.Appointment;

public record UpdateAppointmentRequest
(
    int AppointmentId,
    string Username,
    string Title,
    DateTime AppointmentTime
);