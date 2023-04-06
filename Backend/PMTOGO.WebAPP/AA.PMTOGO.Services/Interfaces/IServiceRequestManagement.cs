using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Services.Interfaces
{
    public interface IServiceRequestManagement
    {
        Task<Result> AcceptRequest(Guid requestId);
        Task<Result> DeclineRequest(Guid id, string username);
        Task<Result> GatherServiceRequests(string username);
        Task<ServiceRequest> CreateUserService(Guid requestId);
    }
}
