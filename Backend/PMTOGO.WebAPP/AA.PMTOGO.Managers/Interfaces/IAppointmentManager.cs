using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces;

public interface IAppointmentManager {
    public Task<Result> GetUserAppointments(string username);
    public Task<Result> InsertAppointment(Appointment appointment, string username);
    public Task<Result> UpdateAppointment(Appointment appointment);
    public Task<Result> DeleteAppointment(int appointmentId);
}