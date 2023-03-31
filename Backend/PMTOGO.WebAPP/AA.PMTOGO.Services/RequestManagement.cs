using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System;


namespace AA.PMTOGO.Services
{
    public class RequestManagement : IRequestManagement
    {
        RequestDAO _requestDAO = new RequestDAO();

        public async Task<Result> AcceptRequest(Guid requestId)
        {//add service request and delete from requested service and return new lsit of service request
            ServiceRequest service = await CreateUserService(requestId);
            
            Result result = new Result();
         
            Result add = await _requestDAO.AddUserService(service.RequestId, service.ServiceName, service.ServiceType, service.ServiceDescription,
                service.ServiceFrequency,service.ServiceProviderEmail, service.ServiceProviderName, service.PropertyManagerEmail, service.PropertyManagerName);

            if (add.IsSuccessful == false)
            {
                result = add;
                return result;
            }
            else
            {
                result = await DeclineRequest(service.RequestId, service.ServiceProviderEmail);

            }
            return result;
        }

        public async Task<Result> DeclineRequest(Guid id, string username)
        {
            Result result1 = new Result();
            Result result = await _requestDAO.DeleteServiceRequest(id);
            if (result.IsSuccessful == true)
            {
                result1 = await _requestDAO.GetServiceRequest(username);
            }
            else
            {
                result1 = result;
            }
            return result1;
        }

        public async Task<Result> GatherServiceRequest(string username)
        {
            
            Result result = await _requestDAO.GetServiceRequest(username);
            return result;
        }


        public async Task<ServiceRequest> CreateUserService(Guid requestId)
        {
            Result res = await _requestDAO.GetARequest(requestId);
            ServiceRequest request = (ServiceRequest)res.Payload!;

            return request;
        }

    }
    
}
