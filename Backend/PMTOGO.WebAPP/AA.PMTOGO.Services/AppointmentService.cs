using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentDAO _appointmentDAO;

        public AppointmentService(IAppointmentDAO appointmentDAO)
        {
            _appointmentDAO = appointmentDAO;
        }

        public Task<Result> DeleteAsync(int appointmentId) => _appointmentDAO.DeleteAsync(appointmentId);

        public Task<Result> GetAllByUserIdAsync(string username) => _appointmentDAO.GetAllByUserIdAsync(username);

        public Task<Result> GetAsync(int appointmentId) => _appointmentDAO.GetAsync(appointmentId);

        public Task<Result> InsertAsync(Appointment appointment, string username) => _appointmentDAO.InsertAsync(appointment, username);

        public Task<Result> UpdateAsync(Appointment appointment) => _appointmentDAO.UpdateAsync(appointment);
    }
}
