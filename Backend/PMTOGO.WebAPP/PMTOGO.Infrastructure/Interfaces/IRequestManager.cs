using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IRequestManager
    {
        Task<Result> AcceptServiceRequest(ServiceRequest request);//update accept
        Task<Result> AddServiceToUser(UserService userService);
        Task<Result> RemoveServiceRequest(ServiceRequest request); // update decline
        Task<Result> GetUserRequest(string username);//get all request for service provider user
    }
}
