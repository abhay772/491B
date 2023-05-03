using AA.PMTOGO.Logging;
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
        private readonly ILogger? _logger;


        public ServiceRequestManager(IServiceRequestManagement serviceRequest, ILogger logger)
        {
            _serviceRequest = serviceRequest;
            _logger = logger;

        }

        //update accept
        public async Task<Result> AcceptServiceRequest(string requestId)
        {
            Result result = new Result();
            try
            {
                Guid id = new Guid(requestId);
                result = await _serviceRequest.AcceptRequest(id);
                await _logger!.Log("AccpetServiceRequest", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Accept User Request Unsuccessful. Try Again Later";
                await _logger!.Log("AcceptServiceRequest", 4, LogCategory.Business, result);
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
                await _logger!.Log("RemoveServiceRequest", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Remove Service Request Unsuccessful. Try Again Later";
                await _logger!.Log("RemoveServiceRequest", 4, LogCategory.Business, result);
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
                await _logger!.Log("GetUserRequests", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load User Request Unsuccessful. Try Again Later";
                await _logger!.Log("GetUserRequests", 4, LogCategory.Business, result);
            }
            return result;
        }
        //service provider accepted frequency change
        public async Task<Result> AcceptFrequencyChange(string requestId, string frequency, string username)
        {
            Result result = new Result();
            Guid id = new Guid(requestId);
            try
            {
                //update user services
                result = await _serviceRequest.FrequencyChange(id,frequency, username);
                await _logger!.Log("AcceptFrequencyChange", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Accept Frequency Change Unsuccessful. Try Again Later";
                await _logger!.Log("AcceptFrequencyChange", 4, LogCategory.Business, result);
            }
            return result;
        }

        public async Task<Result> AcceptCancel(string requestId, string username)
        {
            Result result = new Result();
            Guid id = new Guid(requestId);
            try
            {
                //update user services
                result = await _serviceRequest.CancelUserService(id, username);
                await _logger!.Log("AcceptCancel", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Accept Cancellation Unsuccessful. Try Again Later";
                await _logger!.Log("AcceptCancel", 4, LogCategory.Business, result);
            }
            return result;
        }
    }
}
