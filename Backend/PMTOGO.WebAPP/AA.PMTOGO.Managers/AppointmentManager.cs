
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Managers;

public class AppointmentManager : IAppointmentManager
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentManager(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    } 

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