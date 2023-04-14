﻿using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IServiceManager
    {
        Task<Result> RateUserService(string id, int rate, string role);// rate service
        Task<Result> GetAllServices();//get all services

        Task<Result> GetAllUserServices(string username, string role);//get all user services

        Task<Result> AddServiceRequest(ServiceRequest service, string username);//need to get propertyManager info and address
        Task<Result> FrequencyChangeRequest(string id, string frequency);
        Task<Result> CancelRequest(string id);
    }
}
