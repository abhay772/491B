using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Managers
{
    //input validation, logging
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
            Result result = new Result();
            try
            {
                Guid id = new Guid(requestId);
                result = await _serviceRequest.AcceptRequest(id);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Accept User Request Unsuccessful. Try Again Later";
            }
            return result;
        }

        // update decline
        public async Task<Result> RemoveServiceRequest(string requestId, string email)
        {
            Result result = new Result();
            try
            {
                Guid id = new Guid(requestId);
                result = await _serviceRequest.DeclineRequest(id, email);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Remove Service Request Unsuccessful. Try Again Later";
            }
            return result;
        }
        
        //get all request for service provider user    
        public async Task<Result> GetUserRequests(string username)
        {
            Result result = new Result();
            try
            {
                result = await _serviceRequest.GatherServiceRequests(username);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load User Request Unsuccessful. Try Again Later";
            }
            return result;
        }
        //service provider accepted frequency change
        public async Task<Result> AcceptFrequencyChange(string requestId, string frequency)
        {
            Result result = new Result();
            Guid id = new Guid(requestId);
            try
            {
                //update user services
                result = await _serviceRequest.FrequencyChange(id,frequency);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Accept Frequency Change Unsuccessful. Try Again Later";
            }
            return result;
        }

        public async Task<Result> AcceptCancel(string requestId)
        {
            Result result = new Result();
            Guid id = new Guid(requestId);
            try
            {
                //update user services
                result = await _serviceRequest.CancelUserService(id);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Accept Cancellation Unsuccessful. Try Again Later";
            }
            return result;
        }
    }
}
