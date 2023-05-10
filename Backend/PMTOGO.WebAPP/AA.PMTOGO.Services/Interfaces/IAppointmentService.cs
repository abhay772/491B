using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces;

public interface IAppointmentService {
    public Task<Appointment?> GetAsync(int appointmentId);
    public Task<List<Appointment>> GetAllByUserIdAsync(string username);
    public Task<bool> InsertAsync(Appointment appointment);
    public Task<bool> UpdateAsync(Appointment appointment);
    public Task<bool> DeleteAsync(int appointmentId);
    public Task<User?> GetUserAsync(string username);
}