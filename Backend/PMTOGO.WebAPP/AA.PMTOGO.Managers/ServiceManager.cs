using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Managers
{
    public class ServiceManager: IServiceManager
    {
        private readonly IServiceManagement _service;

        public ServiceManager(IServiceManagement service) 
        {
            _service = service;
        }
        // rate service
        public async Task<Result> RateUserService(UserService service, int rate)
        {
            Result result = await _service.RateService(service.ServiceId, rate);

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
            Result result = await _service.CreateRequest(service, username, comments , frequency); 
            return result;

        }

        public async Task<Result> GetAllUserServices(string username)
        {
            Result result = await _service.GatherUserServices(username);

            return result;
        }


    }
}
