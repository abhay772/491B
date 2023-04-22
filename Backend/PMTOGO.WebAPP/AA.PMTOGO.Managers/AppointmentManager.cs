
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Managers;

public class AppointmentManager : IAppointmentManager
{
    private readonly IAppointmentService _appointmentService;
    private readonly IUserManagement _userService;

    public AppointmentManager(
        IAppointmentService appointmentService,
        IUserManagement userService    
    )
    {
        _appointmentService = appointmentService;
        _userService = userService;
    } 

    public Task<Result> DeleteAppointment(int appointmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> GetUserAppointments(string username)
    {
        var result = new Result();
        var user = await _userService.GetUser(username);

        if (user is null)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Unable to fetch user.";

            return result;
        }

        var appointments = _appointmentService.GetAllByUserIdAsync(user.Username);

        result.IsSuccessful = true;
        result.Payload = appointments;

        return result;
    }

    public Task<Result> InsertAppointment(Appointment appointment, string username)
    {

    }

    public Task<Result> UpdateAppointment(Appointment appointment)
    {
        throw new NotImplementedException();
    }
}