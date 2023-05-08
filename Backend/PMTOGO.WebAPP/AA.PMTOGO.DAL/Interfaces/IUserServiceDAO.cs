using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.DAL.Interfaces
{
    public interface IUserServiceDAO
    {
        Task<Result> FindUserService(Guid id);
        Task<Result> AddUserService(ServiceRequest service);
        Task<Result> GetUserServices(string sqlQuery, string email, string rating);
        Task<Result> UpdateServiceFrequency(Guid id, string frequency);
        Task<Result> DeleteUserService(Guid requestId);
        Task<Result> UpdateServiceRate(Guid Id, int rating, string query);
        Task<Result> CheckRating(Guid Id, int rating, string sqlQuery, string userRate);
        Task<Result> CheckFrequency(Guid id, string frequency);
        Task<Result> UpdateStatus(Guid id, string status);

    }
}
