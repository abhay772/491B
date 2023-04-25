using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Services.Interfaces
{
    public interface IUserServiceManagement
    {
        Task<Result> Rate(Guid serviceId, int rate, string role);
        Task<Result> CreateRequest(Guid id, string type, string frequency);
        Task<Result> AddRequest(Guid id, string frequency, string comments, string username);

        Task<Result> GatherServices();
        Task<Result> CreateService(Service service);
        Task<Result> RemoveService(Service service);
        Task<Result> RequestFrequencyChange(Guid id, string frequency, string type);
        Task<Result> CancellationRequest(Guid id, string frequency, string type);
        Task<Result> ChangeStatus(Guid id, string status);

        Task<Result> GatherUserServices(string username, string role);

        bool CheckRate(int rate);
    }
}
