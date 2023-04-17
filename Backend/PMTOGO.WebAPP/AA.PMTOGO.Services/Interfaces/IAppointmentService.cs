using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces;

public interface IAppointmentService {
    public Task<Result> GetAsync(int appointmentId);
    public Task<Result> GetAllByUserIdAsync(string username);
    public Task<Result> InsertAsync(Appointment appointment, string username);
    public Task<Result> UpdateAsync(Appointment appointment);
    public Task<Result> DeleteAsync(int appointmentId);
}