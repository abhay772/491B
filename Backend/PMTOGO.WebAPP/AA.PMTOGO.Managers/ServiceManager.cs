using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System.Data;

namespace AA.PMTOGO.Managers
{
    //input validation, error handling , logging
    public class ServiceManager: IServiceManager
    {
        private readonly IUserServiceManagement _userService;

        public ServiceManager(IUserServiceManagement userService) 
        {
            _userService = userService;
        }
        // rate service
        public async Task<Result> RateUserService(string serviceId, int rate, string role)
        {
            Guid id = new Guid(serviceId);
            Result result = new Result();
            try
            {
                result = await _userService.Rate(id, rate, role);
                return result;
            }
            catch
            {
                result.IsSuccessful= false;
                result.ErrorMessage = "Rate Unsuccessful";
            }

            return result;
        }
        //get all services
        public async Task<Result> GetAllServices()
        {
            Result result = new Result();
            try
            {
                await _userService.GatherServices();
                return result;
            }
            catch
            {
                result.IsSuccessful= false;
                result.ErrorMessage = "Load services Unsuccessful";
                
            }

            return result;
        }

        public async Task<Result> AddServiceRequest(ServiceRequest service, string username)
        {
            Result result = new Result();
            try
            {
                result = await _userService.CreateRequest(service, username);
                return result;
            }
            catch
            {
                result.IsSuccessful= false;
                result.ErrorMessage = "Add Service Request Unsuccessful";

            }
            return result;

        }

        public async Task<Result> GetAllUserServices(string username, string role)
        {
            Result result = new Result();
            try
            {
                result = await _userService.GatherUserServices(username, role);
                return result;
            }
            catch
            {
                result.IsSuccessful= false;
                result.ErrorMessage = "Load User Services Unsuccssful";
            }

            return result;
        }

        public async Task<Result> FrequencyChangeRequest(string id, string frequency)
        {
            Guid requestId = new Guid(id);
            Result result = new Result();
            try
            {
                result = await _userService.FrequencyChange(requestId, frequency, "Frequency Change");
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load User Services Unsuccssful";
            }

            return result;
        }
    }
}
