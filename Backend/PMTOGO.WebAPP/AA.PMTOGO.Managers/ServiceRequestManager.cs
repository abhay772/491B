using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Managers
{
    public class ServiceRequestManager : IServiceRequestManager
    {
        //ERROR HANDLING
        private readonly IServiceRequestManagement _serviceRequest;


        public ServiceRequestManager(IServiceRequestManagement serviceRequest)
        {
            _serviceRequest = serviceRequest;

        }

        //update accept
        public async Task<Result> AcceptServiceRequest(string requestId)
        {
            Guid id = new Guid(requestId);
            Result result = await _serviceRequest.AcceptRequest(id);


            return result;
        }

        // update decline
        public async Task<Result> RemoveServiceRequest(string requestId, string email)
        {
            Guid id= new Guid(requestId);
            Result result = await _serviceRequest.DeclineRequest(id, email);


            return result;
        }
        
        //get all request for service provider user    
        public async Task<Result> GetUserRequests(string username)
        {
            Result result = await _serviceRequest.GatherServiceRequests(username);

            return result;
        }
    }
}
