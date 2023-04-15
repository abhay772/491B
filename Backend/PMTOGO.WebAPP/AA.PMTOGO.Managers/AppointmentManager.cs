
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers;

public class AppointmentManager : IAppointmentManager
{
    public Task<Result> DeleteAppointment(int appointmentId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> GetUserAppointments(string username)
    {
        throw new NotImplementedException();
    }

    public Task<Result> InsertAppointment(Appointment appointment, string username)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAppointment(Appointment appointment)
    {
        throw new NotImplementedException();
    }
}