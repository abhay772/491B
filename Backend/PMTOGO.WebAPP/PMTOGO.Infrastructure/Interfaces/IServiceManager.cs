using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IServiceManager
    {
        Task<Result> RateUserService(UserService service, int rate);// rate service
        Task<Result> GetAllServices();//get all services

        Task<Result> GetAllUserServices(string username);//get all user services

        Task<Result> AddServiceRequest(Service service, string username, string comments, string frequency);//need to get propertyManager info and address
    }
}
