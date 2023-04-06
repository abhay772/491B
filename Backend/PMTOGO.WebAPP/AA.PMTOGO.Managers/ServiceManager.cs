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
        public async Task<Result> RateUserService(UserService service, int rate)
        {
            Result result = await _userService.RateService(service.Id, rate);

            return result;
        }
        //get all request for service provider user    
        public async Task<Result> GetAllServices()
        {
            Result result = await _service.GatherServices();

            return result;
        }

        public async Task<Result> AddServiceRequest(Service service, string username, string comments, string frequency)
        {   
            Result result = await _userService.CreateRequest(service, username, comments , frequency); 
            return result;

        }

        public async Task<Result> GetAllUserServices(string username)
        {
            Result result = await _userService.GatherUserServices(username);

            return result;
        }


    }
}
