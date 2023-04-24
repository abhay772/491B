
namespace AA.PMTOGO.WebAPP.Contracts.Appointment;

public record UpdateAppointmentRequest
(
    int AppointmentId,
    string Title,
    DateTime AppointmentTime
);