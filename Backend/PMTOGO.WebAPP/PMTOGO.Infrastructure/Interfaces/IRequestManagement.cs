using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IRequestManagement
    {
        Task<Result> AcceptRequest(Guid requestId);
        Task<Result> DeclineRequest(Guid id, string username);
        Task<Result> GatherServiceRequest(string username);
        Task<ServiceRequest> CreateUserService(Guid requestId);
    }
}
