using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL.Interfaces;

public interface IAppointmentDAO {
    public Task<Appointment?> GetAsync(int appointmentId);
    public Task<List<Appointment>> GetAllByUserIdAsync(string username);
    public Task<List<Appointment>> GetAllUpcomingByUserIdAsync(string username);
    public Task<bool> InsertAsync(Appointment appointment);
    public Task<bool> UpdateAsync(Appointment appointment);
    public Task<bool> DeleteAsync(int appointmentId);
    public Task<User?> GetUserAsync(string username);
}