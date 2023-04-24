using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Services.Interfaces
{
    public interface IServiceRequestManagement
    {
        Task<Result> AcceptRequest(Guid requestId);
        Task<Result> DeclineRequest(Guid id, string username);
        Task<Result> GatherServiceRequests(string username);
        Task<ServiceRequest> CreateUserService(Guid requestId);
        Task<Result> AddRequest(ServiceRequest request);
        Task<Result> FrequencyChange(Guid id, string frequency, string email);
        Task<Result> CancelUserService(Guid id, string email);
    }
}
