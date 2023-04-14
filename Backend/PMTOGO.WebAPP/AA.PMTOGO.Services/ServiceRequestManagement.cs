using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;


namespace AA.PMTOGO.Services
{
    //input validation, logging
    public class ServiceRequestManagement : IServiceRequestManagement
    {
        ServiceRequestDAO _requestDAO = new ServiceRequestDAO();
        UserServiceDAO _serviceDAO= new UserServiceDAO();

        public async Task<Result> AcceptRequest(Guid requestId)
        {
            //add service request and delete from requested services and return new list of service request
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

            Result result = new Result();
            try
            {
                result = await _requestDAO.GetServiceRequests(username);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load Service Request Unsuccessful. Try Again Later";
            }
            return result;
        }


        public async Task<ServiceRequest> CreateUserService(Guid requestId)
        {
            Result result = new Result();
            try
            {
                result = await _requestDAO.GetAServiceRequest(requestId);
                ServiceRequest request = (ServiceRequest)result.Payload!;
                return request;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Could not find User Service. Try Again Later";
            }
            
            return null!;
        }

        public async Task<Result> AddRequest(ServiceRequest request)
        {
            Result result = new Result();
            try
            {
                result = await _requestDAO.AddServiceRequest(request);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Add Service Request Unsuccessful. Try Again Later";
            }
            return result;

        }
        //if accepted by service provider frequency is updated
        public async Task<Result> FrequencyChange(Guid id, string frequency)
        {
            Result result = new Result();
            try
            {
                result = await _serviceDAO.UpdateServiceFrequency(id, frequency);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Frequency could not be updated";
                return result;

            }
        }


        // if cancellation confirmed by service provider
        public async Task<Result> CancelUserService(Guid id)
        {
            Result result = new Result();
            try
            {
                result = await _serviceDAO.DeleteUserService(id);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "User Service Cancellation Not Successsful";
                return result;

            }
        }

    }
    
}
