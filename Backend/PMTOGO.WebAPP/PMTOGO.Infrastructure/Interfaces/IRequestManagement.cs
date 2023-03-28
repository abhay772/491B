using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IRequestManagement
    {
        Task<Result> AcceptRequest(ServiceRequest service);
        Task<Result> DeclineRequest(Guid id, string username);
        Task<Result> GatherServiceRequest(string username);
        //Task<Result> CreateRequest(ServiceRequest service);
        Task<Result> CreateUserService(UserService service);
    }
}
