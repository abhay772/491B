
using AA.PMTOGO.Libary;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
//using AA.PMTOGO.WebAPP.Contracts.Appointment;
using Microsoft.AspNetCore.Mvc;

namespace AA.PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
       /* private readonly IAppointmentManager _appointmentManager;
        private readonly ClaimValidation _claims;

        public AppointmentController(IAppointmentManager appointmentManager, ClaimValidation claims)
        {
            _appointmentManager = appointmentManager;
            _claims = claims;//uses input validation
        }

#if DEBUG

        [HttpGet]
        [Route("health")]   //make sure controller route works.
        public Task<IActionResult> HealthCheck()
        {
            return Task.FromResult<IActionResult>(Ok("Healthy"));
        }

#endif

        [HttpGet]
        public async Task<IActionResult> GetUserAppointment([FromQuery] string jwt)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation(null!, jwt);
            UserClaims user = (UserClaims)result.Payload!;

            if (result.IsSuccessful)
            {
                try
                {
                    Result userAppointments = await _appointmentManager.GetUserAppointments(user.ClaimUsername);
                    if (userAppointments.IsSuccessful)
                    {
                        return Ok(userAppointments.Payload!);
                    }
                    else
                    {
                        return BadRequest(new { message = "Retry again or contact system admin." });
                    }
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return BadRequest("Cookie not found");
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointmentRequest(InsertAppointmentRequest appointmentRequest, [FromQuery] string jwt)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation(null!, jwt);
            UserClaims user = (UserClaims)result.Payload!;
            var appointment = new Appointment(appointmentRequest.Title, appointmentRequest.AppointmentTime);

            if (result.IsSuccessful)
            {
                try
                {
                    Result insert = await _appointmentManager.InsertAppointment(appointment, user.ClaimUsername);
                    if (insert.IsSuccessful)
                    {
                        return Ok(new { message = insert.Payload });
                    }
                    else
                    {

                        return BadRequest(new { message = "Retry again or contact system admin" });
                    }
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);

                }
            }
            return BadRequest("Invalid Credentials");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppointment(UpdateAppointmentRequest appointmentRequest, [FromQuery] string jwt)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation(null!, jwt);
            UserClaims user = (UserClaims)result.Payload!;
            var appointment = new Appointment(appointmentRequest.AppointmentId, user.ClaimUsername, appointmentRequest.Title, appointmentRequest.AppointmentTime);

            if (result.IsSuccessful)
            {
                try
                {
                    Result rating = await _appointmentManager.UpdateAppointment(appointment);
                    if (rating.IsSuccessful)
                    {
                        return Ok(rating.Payload);
                    }
                    else
                    {

                        return BadRequest("Invalid username or password provided. Retry again or contact system admin" + result.Payload);
                    }
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            return BadRequest("Invalid Credentials");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAppointment(int appointmentId, [FromQuery] string jwt)
        {
            Result result = new Result();
            result = _claims.ClaimsValidation(null!, jwt);

            if (result.IsSuccessful)
            {
                try
                {
                    Result rating = await _appointmentManager.DeleteAppointment(appointmentId);
                    if (rating.IsSuccessful)
                    {
                        return Ok(rating.Payload);
                    }
                    else
                    {

                        return BadRequest("Invalid username or password provided. Retry again or contact system admin" + result.Payload);
                    }
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            return BadRequest("Invalid Credentials");
        }*/
    }
}