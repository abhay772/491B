using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Managers
{
    public class ServiceManager: IServiceManager
    {
        private readonly IRequestManagement _service;
        UsersDAO _authNDAO = new UsersDAO();
        private readonly ILogger? _logger;

        public ServiceManager(IRequestManagement service, ILogger logger) 
        {
            _service = service;
            _logger = logger;
        }

        //update accept
        public async Task<Result> AcceptServiceRequest(ServiceRequest request)
        {
            Result result = await _service.AcceptRequest(request);
            //_logger!.Log("AcceptServiceRequest", 1, LogCategory.Business, result);

            return result;
        }

        // update decline
        public async Task<Result> RemoveServiceRequest(ServiceRequest request)
        {
            Result result = await _service.DeclineRequest(request.ServiceRequestId, request.ServiceProviderEmail);
            //_logger!.Log("RemoveServiceRequest", 1, LogCategory.Business, result);

            return result;
        }
        // rate service
        public async Task<Result> RateUserService(UserService service, int rate)
        {
            Result result = await _service.RateService(service.ServiceId, rate);
            //_logger!.Log("RateUserService", 1, LogCategory.Business, result);
            return result;
        }
        //get all request for service provider user    
        public async Task<Result> GetUserRequest(string username)
        {
            Result result = await _service.GatherServiceRequest(username);
            //_logger!.Log("GetUserRequest", 1, LogCategory.Business, result);
            return result;
        }

        public async Task<Result> AddServiceToUser(string username, UserService userService)
        {
            var result = new Result();
            var user = (await _authNDAO.FindUser(username)).Payload as User;

            if (user is null)
            {
                result.IsSuccessful = false;
                result.Payload = null;
                result.ErrorMessage = "User cannot be found.";

                return result;
            }
            else
            {
                user.Services.Add(userService);

                result.IsSuccessful = true;
                result.Payload = user;
                result.ErrorMessage = null;

                return result;
            }
        }

        public async Task<Result> RequestAService(ServiceRequest serviceRequest)
        {
            Result result = await _service.CreateRequest(serviceRequest);
            return result;

        }
    }
}
