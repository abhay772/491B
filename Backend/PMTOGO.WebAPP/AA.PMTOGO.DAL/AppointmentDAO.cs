using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Infrastructure.Data;
using AA.PMTOGO.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AA.PMTOGO.DAL
{
    public class AppointmentDAO : IAppointmentDAO
    {
        private readonly UsersDbContext _dbContext;

        public AppointmentDAO(
            UsersDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }
        
        public async Task<bool> DeleteAsync(int appointmentId)
        {
            var origin = await _dbContext.Appointment.FindAsync(appointmentId);

            if (origin is null) return false;

            origin.IsActive = false;
            var result =  _dbContext.Appointment.Update(origin);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Appointment>> GetAllByUserIdAsync(string username)
        {
            var result = await _dbContext.Appointment
                .Where(x => x.Username == username && x.IsActive == true)
                .ToListAsync();

            return result;
        }

        public async Task<List<Appointment>> GetAllUpcomingByUserIdAsync(string username)
        {
            var result = await _dbContext.Appointment
                .Where(x => x.Username == username && x.IsActive == true && x.AppointmentTime > DateTime.UtcNow)
                .ToListAsync();

            return result;
        }

        public async Task<Appointment?> GetAsync(int appointmentId)
        {
            var result = await _dbContext.Appointment.FindAsync(appointmentId);

            return result;
        }

        public async Task<bool> InsertAsync(Appointment appointment)
        {
            appointment.AppointmentId = 0;
            appointment.IsActive = true;

            _dbContext.Appointment.Add(appointment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Appointment appointment)
        {
            _dbContext.Appointment.Update(appointment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<User?> GetUserAsync(string username)
        {
            // var result = await _dbContext.Database
            //     .SqlQuery<User>($"SELECT [u].[Username], [u].[Attempts], [u].[IsActive], [u].[OTP], [u].[OTPTimestamp], [u].[PassDigest], [u].[RecoveryRequest], [u].[Role], [u].[Salt], [u].[Timestamp] FROM [UserAccounts] as [u] WHERE [a].[Username] = {username} AND [a].[IsActive] = CAST(1 AS bit)")
            //     .FirstAsync();

            var result2 = await _dbContext.User
                .Where(x => x.Username == username)
                .Select(x => new User()
                {
                    Username = x.Username,
                    Role = x.Role,
                    PassDigest = x.PassDigest,
                    Salt = x.Salt,
                    IsActive = x.IsActive,
                    Attempt = x.Attempt,
                    Timestamp = x.Timestamp,
                    OTP = x.OTP,
                    OTPTimestamp = x.OTPTimestamp,
                    RecoveryRequest = x.RecoveryRequest
                })
                .FirstOrDefaultAsync();

            return result2;
        }
    }
}
