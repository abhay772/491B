using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Services.Interfaces
{
    public interface IUserServiceManagement
    {
        Task<Result> CreateRequest(Guid id, string type, string frequncy);
        Task<Result> AddRequest(Guid id, string frequency, string comments, string username);
        Task<Result> GatherUserServices(string username, string role);

        Task<Result> Rate(Guid serviceId, int rate, string role);

        bool CheckRate(int rate);
        Task<Result> GatherServices();

        Task<Result> GatherSPServices(string username);
        Task<Result> CreateService(Service service);
        Task<Result> RemoveService(Guid id);
        Task<Result> RequestFrequencyChange(Guid id, string frequency, string type);
        Task<Result> CancellationRequest(Guid id, string frequency, string type);

    }
}