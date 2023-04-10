using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Services.Interfaces
{
    public interface IUserServiceManagement
    {
        Task<Result> RateService(Guid serviceId, int rate);
        Task<Result> CreateRequest(ServiceRequest service,  string username);
        Task<Result> AddRequest(ServiceRequest request);
        Task<Result> GatherUserServices(string username);

        bool CheckRate(int rate);
    }
}
