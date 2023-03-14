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
        Task<Result> AcceptServiceRequest(ServiceRequest request);//update accept
        Task<Result> AddServiceToUser(string username, UserService userService);
        Task<Result> RemoveServiceRequest(ServiceRequest request); // update decline
        Task<Result> RateUserService(UserService service, int rate);// rate service
        Task<Result> GetUserRequest(string username);//get all request for service provider user
        Task<Result> RequestAService(ServiceRequest serviceRequest);


    }
}
