using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Managers
{
    public class ServiceManager: IServiceManager
    {
        private readonly IServiceManagement _service;
        private readonly IUserServiceManagement _userService;

        public ServiceManager(IServiceManagement service, IUserServiceManagement userService) 
        {
            _service = service;
            _userService = userService;
        }
        // rate service
        public async Task<Result> RateUserService(string serviceId, int rate)
        {
            Guid id = new Guid(serviceId);
            Result result = await _userService.RateService(id, rate);

            return result;
        }
        //get all request for service provider user    
        public async Task<Result> GetAllServices()
        {
            Result result = await _service.GatherServices();

            return result;
        }

        public async Task<Result> AddServiceRequest(ServiceRequest service, string username)
        {   
            Result result = await _userService.CreateRequest(service, username); 
            return result;

        }

        public async Task<Result> GetAllUserServices(string username)
        {
            Result result = await _userService.GatherUserServices(username);

            return result;
        }


    }
}
