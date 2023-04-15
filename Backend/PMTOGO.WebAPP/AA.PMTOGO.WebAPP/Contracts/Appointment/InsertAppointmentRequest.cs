
namespace AA.PMTOGO.WebAPP.Contracts.Appointment;

public record InsertAppointmentRequest
(
    string Title,
    DateTime AppointmentTime
);