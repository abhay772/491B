using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IServiceManagement
    {
        Task<Result> GatherServices();

        Task<Result> GatherUserServices(string username);
        Task<Result> RateService(Guid serviceId, int rate);
        Task<Result> CreateRequest(Service service,  string username, string comments, string frequency);
    }
}
