using AA.PMTOGO.DAL;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Channels;


namespace AA.PMTOGO.Services
{
    //input validation, logging
    public class ServiceRequestManagement : IServiceRequestManagement
    {
        ServiceRequestDAO _requestDAO = new ServiceRequestDAO();
        UserServiceDAO _serviceDAO= new UserServiceDAO();
        private readonly ILogger _logger;

        public ServiceRequestManagement(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<Result> AcceptRequest(Guid requestId)
        {
            Result result = new Result();
            try
            {
                //add service request and delete from requested services and return new list of service request
                ServiceRequest service = await CreateUserService(requestId);

                Result insert = await _serviceDAO.AddUserService(service);

                if (insert.IsSuccessful == false)
                {
                    await _logger!.Log("AcceptRequest", 4, LogCategory.Business, result);
                    return insert;
                }
                else
                {
                    result = await DeclineRequest(service.Id, service.ServiceProviderEmail);
                    await _logger!.Log("AcceptRequest", 4, LogCategory.Business, result);

                }
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Could not find User Service. Try Again Later";
                await _logger!.Log("CreateUserService", 4, LogCategory.Business, result);

            }
            return result;
        }

        public async Task<Result> DeclineRequest(Guid id, string username)
        {
            Result result = new Result();
            try
            {
                Result delete = await _requestDAO.DeleteServiceRequest(id);

                if (delete.IsSuccessful == true)
                {
                    await _serviceDAO.UpdateStatus(id, "In-Progress");
                    await _logger!.Log("DeclineRequest", 4, LogCategory.Business, result);
                    result = await _requestDAO.GetServiceRequests(username);
                    return result;

                }
                else
                {
                    await _logger!.Log("DeclineRequest", 4, LogCategory.Business, result);
                    delete = result;
                    return result;
                    
                }
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Could not find User Service. Try Again Later";
                await _logger!.Log("DeclineServiceRequest", 4, LogCategory.Business, result);

            }
            return result;
        }

        public async Task<Result> GatherServiceRequests(string username)
        {

            Result result = new Result();
            try
            {
                result = await _requestDAO.GetServiceRequests(username);
                await _logger!.Log("GatherServiceRequests", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load Service Request Unsuccessful. Try Again Later";
                await _logger!.Log("GatherServiceRequests", 4, LogCategory.Business, result);
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
                await _logger!.Log("CreateUserService", 4, LogCategory.Business, result);
                return request;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Could not find User Service. Try Again Later";
                await _logger!.Log("CreateUserService", 4, LogCategory.Business, result);
            }
            
            return null!;
        }

        public async Task<Result> AddRequest(ServiceRequest request)
        {
            Result result = new Result();
            try
            {
                result = await _requestDAO.AddServiceRequest(request);
                await _logger!.Log("AddRequest", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Add Service Request Unsuccessful. Try Again Later";
                await _logger!.Log("AddRequest", 4, LogCategory.Business, result);
            }
            return result;

        }
        //if accepted by service provider frequency is updated
        public async Task<Result> FrequencyChange(Guid id, string frequency, string username)
        {
            Result result = new Result();
            try
            {
                result = await _serviceDAO.UpdateServiceFrequency(id, frequency);
                if (result.IsSuccessful) 
                {
                    await _serviceDAO.UpdateStatus(id, "In-Progress");
                    await _logger!.Log("FrequencyChange", 4, LogCategory.Business, result);
                    result = await DeclineRequest(id, username); //delete the request from service request list

                }
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Frequency could not be updated";
                await _logger!.Log("FrequencyChange", 4, LogCategory.Business, result);
                return result;

            }
        }


        // if cancellation confirmed by service provider
        public async Task<Result> CancelUserService(Guid id, string username)
        {
            Result result = new Result();
            try
            {
                Result cancel = await _serviceDAO.DeleteUserService(id);
                //log
                if (cancel.IsSuccessful == false)
                {
                    await _logger!.Log("CancelUserService", 4, LogCategory.Business, result);
                    return cancel;
                }
                else
                {
                    await _logger!.Log("CancelUserService", 4, LogCategory.Business, result);
                    result = await DeclineRequest(id, username);

                }
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "User Service Cancellation Not Successsful";
                await _logger!.Log("CancelUserService", 4, LogCategory.Business, result);
                return result;

            }
        }

    }
    
}
