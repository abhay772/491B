
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Managers;

public class AppointmentManager : IAppointmentManager
{
    private readonly IAppointmentService _appointmentService;
    //private readonly IUserManagement _userService;

    public AppointmentManager(
        IAppointmentService appointmentService
    )
    {
        _appointmentService = appointmentService;
    } 

    public async Task<Result> DeleteAppointment(int appointmentId)
    {
        var result = new Result();
        var origin = await _appointmentService.GetAsync(appointmentId);

        if (origin is null)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Appointment not found.";

            return result;
        }

        var deleteResult = await _appointmentService.DeleteAsync(appointmentId);

        if (deleteResult == false)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "An error occured.";

            return result;
        }

        result.IsSuccessful = true;
        result.Payload = true;

        return result;
    }

    public async Task<Result> GetUserAppointments(string username)
    {
        var result = new Result();
        var user = await _appointmentService.GetUserAsync(username);

        if (user is null)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Unable to fetch user.";

            return result;
        }

        var appointments = await _appointmentService.GetAllByUserIdAsync(user.Username);

        result.IsSuccessful = true;
        result.Payload = appointments;

        return result;
    }

    public async Task<Result> InsertAppointment(Appointment appointment, string username)
    {
        var result = new Result();

        if (appointment.AppointmentTime < DateTime.UtcNow)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid appointment time.";

            return result;
        }

        var user = await _appointmentService.GetUserAsync(username);

        if (user is null)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Unable to fetch user.";

            return result;
        }

        var appointments = await _appointmentService.GetAllByUserIdAsync(user.Username);

        //validate appointment times
        foreach (var apmt in appointments)
        {
            var isInvalidTime = appointment.AppointmentTime > apmt.AppointmentTime.AddHours(-1) && appointment.AppointmentTime < apmt.AppointmentTime.AddHours(1);

            if (isInvalidTime == true)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid appointment time.";

                return result;
            }
        }

        appointment.Username = username;
        var insertResult = await _appointmentService.InsertAsync(appointment);

        if (insertResult == false)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "An error has occured.";

            return result;
        }

        result.IsSuccessful = true;
        result.Payload = true;

        return result;
    }

    public async Task<Result> UpdateAppointment(Appointment appointment)
    {
        var result = new Result();

        if (appointment.AppointmentTime < DateTime.UtcNow)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid appointment time.";

            return result;
        }

        var appointments = await _appointmentService.GetAllByUserIdAsync(appointment.Username);

        //validate appointment times
        foreach (var apmt in appointments)
        {
            var isInvalidTime = appointment.AppointmentTime > apmt.AppointmentTime.AddHours(-1) && appointment.AppointmentTime < apmt.AppointmentTime.AddHours(1);

            if (isInvalidTime == true)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid appointment time.";

                return result;
            }
        }

        var updateResult = await _appointmentService.UpdateAsync(appointment);

        if (updateResult == false)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "An error has occured.";

            return result;
        }

        result.IsSuccessful = true;
        result.Payload = true;

        return result;
    }
}