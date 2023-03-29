using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Managers
{
    public class RequestManager : IRequestManager
    {
        private readonly IRequestManagement _service;


        public RequestManager(IRequestManagement service)
        {
            _service = service;

        }

        //update accept
        public async Task<Result> AcceptServiceRequest(ServiceRequest request)
        {
            Result result = await _service.AcceptRequest(request);


            return result;
        }

        // update decline
        public async Task<Result> RemoveServiceRequest(ServiceRequest request)
        {
            Result result = await _service.DeclineRequest(request.RequestId, request.ServiceProviderEmail);


            return result;
        }
        //get all request for service provider user    
        public async Task<Result> GetUserRequest(string username)
        {
            Result result = await _service.GatherServiceRequest(username);

            return result;
        }

        public async Task<Result> AddServiceToUser(UserService service)
        {
            Result result = await _service.CreateUserService(service);
            return result;
        }

        /*public async Task<Result> RequestAService(ServiceRequest serviceRequest)
        {
            Result result = await _service.CreateRequest(serviceRequest);
            return result;

        }*/
    }
}
