using AA.PMTOGO.Logging;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System.Data;

namespace AA.PMTOGO.Managers
{
    //input validation, logging
    public class ServiceManager: IServiceManager
    {
        private readonly IUserServiceManagement _userService;
        private readonly ILogger _logger;

        public ServiceManager(IUserServiceManagement userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
        }
        // rate service
        public async Task<Result> RateUserService(string serviceId, int rate, string role)
        {
            Guid id = new Guid(serviceId);
            Result result = new Result();
            try
            {
                result = await _userService.Rate(id, rate, role);
                await _logger!.Log("RateUserService", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful= false;
                result.ErrorMessage = "Rate Unsuccessful. Try Again Later";
                await _logger!.Log("RateUserService", 4, LogCategory.Business, result);
            }

            return result;
        }
        //get all services
        public async Task<Result> GetAllServices()
        {
            Result result = new Result();
            try
            {
                result = await _userService.GatherServices();
                result.ErrorMessage = "Gather Services Successful";
                await _logger!.Log("GetAllServices", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful= false;
                result.ErrorMessage = "Load services Unsuccessful. Try Again Later";
                await _logger!.Log("GetAllServices", 4, LogCategory.Business, result);

            }

            return result;
        }
        //get service provider services
        public async Task<Result> GetSPServices(string username)
        {
            Result result = new Result();
            try
            {
                result = await _userService.GatherSPServices(username);
                await _logger!.Log("GetSPServices", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load services Unsuccessful. Try Again Later";
                await _logger!.Log("GetSPServices", 4, LogCategory.Business, result);

            }

            return result;
        }
        //create service provider services
        public async Task<Result> AddSPService(string username, string serviceName, string serviceType, string serviceDescription, double servicePrice)
        {
            Result result = new Result();
            try
            {
                Service service = new Service(serviceName, serviceType, serviceDescription, servicePrice);
                result = await _userService.CreateService(username, service);
                await _logger!.Log("AddSPServices", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load services Unsuccessful. Try Again Later";
                await _logger!.Log("AddSPServices", 4, LogCategory.Business, result);

            }

            return result;
        }

        //delete service provider services
        public async Task<Result> DeleteSPService(string id)
        {
            Guid Id = new Guid(id);
            Result result = new Result();
            try
            {
                result = await _userService.RemoveService(Id);
                await _logger!.Log("DeleteSPServices", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load services Unsuccessful. Try Again Later";
                await _logger!.Log("DeleteSPServices", 4, LogCategory.Business, result);

            }

            return result;
        }

        public async Task<Result> AddServiceRequest(string Id, string frequency, string comments, string username)
        {
            Guid id = new Guid(Id);
            Result result = new Result();
            try
            {
                Result add = await _userService.AddRequest(id, frequency, comments, username);
                await _logger!.Log("AddServiceRequest", 4, LogCategory.Business, result);
                return add;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Add Service Request Unsuccessful. Try Again Later";
                await _logger!.Log("AddServiceRequest", 4, LogCategory.Business, result);

            }
            return result;

        }

        public async Task<Result> GetAllUserServices(string username, string role)
        {
            Result result = new Result();
            try
            {
                result = await _userService.GatherUserServices(username, role);
                await _logger!.Log("GetAllUserServices", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful= false;
                result.ErrorMessage = "Load User Services Unsuccssful. Try Again Later";
                await _logger!.Log("GetAllUserServices", 4, LogCategory.Business, result);
            }

            return result;
        }

        public async Task<Result> FrequencyChangeRequest(string id, string frequency)
        {
            Guid requestId = new Guid(id);
            Result result = new Result();
            try
            {
                result = await _userService.RequestFrequencyChange(requestId, frequency, "Frequency Change");
                await _logger!.Log("FrequencyChangeRequest", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load User Services Unsuccssful. Try again Later";
                await _logger!.Log("FrequencyChangeRequest", 4, LogCategory.Business, result);
            }

            return result;
        }

        public async Task<Result> CancelRequest(string id)
        {
            Guid requestId = new Guid(id);
            Result result = new Result();
            try
            {
                result = await _userService.CancellationRequest(requestId, "0", "Cancellation");
                await _logger!.Log("CancelRequest", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Cancel Request Unsuccssful. Try Again Later";
                await _logger!.Log("CancelRequest", 4, LogCategory.Business, result);
            }

            return result;
        }
    }
}
