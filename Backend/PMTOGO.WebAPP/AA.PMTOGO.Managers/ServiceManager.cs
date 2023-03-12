﻿using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Managers
{
    public class ServiceManager: IServiceManager
    {
        private readonly IRequestManagement _service;
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
            _logger!.Log("AcceptServiceRequest", 1, LogCategory.Business, result);

            return result;
        }

        // update decline
        public async Task<Result> RemoveServiceRequest(ServiceRequest request)
        {
            Result result = await _service.DeclineRequest(request.ServiceRequestId, request.ServiceProviderEmail);
            _logger!.Log("RemoveServiceRequest", 1, LogCategory.Business, result);

            return result;
        }
        // rate service
        public async Task<Result> RateUserService(UserService service, int rate)
        {
            Result result = await _service.RateService(service.ServiceId, rate);
            _logger!.Log("RateUserService", 1, LogCategory.Business, result);
            return result;
        }
        //get all request for service provider user    
        public async Task<Result> GetUserRequest(string username)
        {
            Result result = await _service.GatherServiceRequest(username);
            _logger!.Log("GetUserRequest", 1, LogCategory.Business, result);
            return result;
        }
    }
}