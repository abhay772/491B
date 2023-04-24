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

        public Task<bool> DeleteAsync(int appointmentId) => _appointmentDAO.DeleteAsync(appointmentId);

        public Task<List<Appointment>> GetAllByUserIdAsync(string username) => _appointmentDAO.GetAllByUserIdAsync(username);

        public Task<Appointment?> GetAsync(int appointmentId) => _appointmentDAO.GetAsync(appointmentId);

        public Task<bool> InsertAsync(Appointment appointment) => _appointmentDAO.InsertAsync(appointment);

        public Task<bool> UpdateAsync(Appointment appointment) => _appointmentDAO.UpdateAsync(appointment);

        public Task<User?> GetUserAsync(string username) => _appointmentDAO.GetUserAsync(username);
    }
}
