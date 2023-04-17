using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.DAL
{
    public class AppointmentDAO : IAppointmentDAO
    {
        
        public Task<Result> DeleteAsync(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetAllByUserIdAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetAsync(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> InsertAsync(Appointment appointment, string username)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
