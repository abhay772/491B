using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Services
{
    public class RequestManagement : IRequestManagement
    {
        RequestDAO _requestDAO = new RequestDAO();

        public async Task<Result> AcceptRequest(ServiceRequest service)
        {//add service request and delete from requested service and return new lsit of service request
            Result result = new Result();
         
            Result add = await _requestDAO.AddService(service.ServiceRequestId, service.PropertyManagerEmail, service.ServiceName, service.ServiceDescription,
                service.ServiceType, service.ServiceFrequency, service.ServiceProviderEmail, service.PropertyManagerName);

            if (add.IsSuccessful == false)
            {
                result = add;
                return result;
            }
            else
            {
                result = await DeclineRequest(service.ServiceRequestId, service.ServiceProviderEmail);

            }
            return result;
        }

        public async Task<Result> DeclineRequest(Guid id, string username)
        {
            Result result1 = new Result();
            Result result = await _requestDAO.DeleteServiceRequest(id);
            if (result.IsSuccessful == true)
            {
                result1 = await _requestDAO.GetUserRequest(username);
            }
            else
            {
                result1 = result;
            }
            return result1;
        }

        public async Task<Result> GatherServiceRequest(string username)
        {
            
            Result result = await _requestDAO.GetUserRequest(username);
            return result;
        }

        public async Task<Result> RateService(Guid id, int rate)
        {
            
            Result result = await _requestDAO.RateUserService(id, rate);
            return result;
        }
        public async Task<Result> CreateRequest(ServiceRequest service)
        {
            Guid serviceRequestId = Guid.NewGuid();
            Result result = await _requestDAO.AddRequest(serviceRequestId, service.ServiceProviderEmail, service.ServiceName, service.ServiceDescription,
                service.ServiceType, service.ServiceFrequency, service.Comments, service.PropertyManagerName, service.PropertyManagerEmail);
            return result;
        }

    }
    
}
