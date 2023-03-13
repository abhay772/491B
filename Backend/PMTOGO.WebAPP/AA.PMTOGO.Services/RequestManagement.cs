using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System;


namespace AA.PMTOGO.Services
{
    public class RequestManagement : IRequestManagement
    {
        RequestDAO _requestDAO = new RequestDAO();

        public async Task<Result> AcceptRequest(ServiceRequest service)
        {//add service request and delete from requested service and return new lsit of service request
            Result result = new Result();
         
            Result add = await _requestDAO.AddService(service.RequestId, service.ServiceName, service.ServiceType, service.ServiceDescription,
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
            Result result = new Result();
            if(CheckRate(rate)) 
            { 
                result = await _requestDAO.RateUserServices(id, rate);
                return result;
            }
            result.IsSuccessful = false;
            result.ErrorMessage = "Rate is out of range. Enter a number between 1-5.";
            return result;
        }
        public async Task<Result> CreateRequest(ServiceRequest request)
        {
            Guid serviceRequestId = Guid.NewGuid();
            Result result = await _requestDAO.AddRequest(serviceRequestId, request.ServiceName, request.ServiceType, request.ServiceDescription,
               request.ServiceFrequency, request.Comments, request.ServiceProviderEmail, request.ServiceProviderName, request.PropertyManagerEmail, request.PropertyManagerName);
            return result;
        }
        public async Task<Result> CreateService(UserService service)
        {
            Guid serviceRequestId = Guid.NewGuid();
            Result result = await _requestDAO.AddService(serviceRequestId, service.ServiceName, service.ServiceType, service.ServiceDescription,
                 service.ServiceFrequency, service.ServiceProviderEmail, service.ServiceProvider, service.PropertyManagerEmail, service.PropertyManagerName);
            return result;
        }
        public bool CheckRate(int rate)
        {
            if(rate > 5)
            {
                return false;
            }
            return true;
        }

    }
    
}
