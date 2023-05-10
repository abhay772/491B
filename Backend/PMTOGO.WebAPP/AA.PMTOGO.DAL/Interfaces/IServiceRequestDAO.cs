using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.DAL.Interfaces
{
    public interface IServiceRequestDAO
    {
        Task<Result> FindServiceRequest(Guid id);
        Task<Result> GetAServiceRequest(Guid requestId);
        Task<Result> GetServiceRequests(string serviceProviderEmail);
        Task<Result> AddServiceRequest(ServiceRequest request);
        Task<Result> DeleteServiceRequest(Guid requestId);

    }
}
