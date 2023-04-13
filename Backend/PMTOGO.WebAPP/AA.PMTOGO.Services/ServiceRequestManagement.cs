using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;


namespace AA.PMTOGO.Services
{
    //input validation, error handling , logging
    public class ServiceRequestManagement : IServiceRequestManagement
    {
        ServiceRequestDAO _requestDAO = new ServiceRequestDAO();
        UserServiceDAO _serviceDAO= new UserServiceDAO();

        public async Task<Result> AcceptRequest(Guid requestId)
        {//add service request and delete from requested service and return new list of service request
            ServiceRequest service = await CreateUserService(requestId);
            
            Result result = new Result();
         
            Result insert = await _serviceDAO.AddUserService(service);

            if (insert.IsSuccessful == false)
            {
                result = insert;
                return result;
            }
            else
            {
                result = await DeclineRequest(service.Id, service.ServiceProviderEmail);

            }
            return result;
        }

        public async Task<Result> DeclineRequest(Guid id, string username)
        {
            Result result1 = new Result();
            Result result = await _requestDAO.DeleteServiceRequest(id);
            if (result.IsSuccessful == true)
            {
                result1 = await _requestDAO.GetServiceRequests(username);
            }
            else
            {
                result1 = result;
            }
            return result1;
        }

        public async Task<Result> GatherServiceRequests(string username)
        {
            
            Result result = await _requestDAO.GetServiceRequests(username);
            return result;
        }


        public async Task<ServiceRequest> CreateUserService(Guid requestId)
        {
            Result result = await _requestDAO.GetAServiceRequest(requestId);
            ServiceRequest request = (ServiceRequest)result.Payload!;

            return request;
        }

        public async Task<Result> AddRequest(ServiceRequest request)
        {
            Result result = await _requestDAO.AddServiceRequest(request);
            return result;

        }

    }
    
}
