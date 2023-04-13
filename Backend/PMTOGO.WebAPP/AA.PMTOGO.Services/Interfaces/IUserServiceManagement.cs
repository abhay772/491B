using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Services.Interfaces
{
    public interface IUserServiceManagement
    {
        Task<Result> CreateRequest(ServiceRequest service,  string username);
        Task<Result> GatherUserServices(string username, string role);

        Task<Result> Rate(Guid serviceId, int rate, string role);

        bool CheckRate(int rate);
        Task<Result> GatherServices();

        Task<Result> CreateService(Service service);


        Task<Result> RemoveService(Service service);

        Task<Result> FrequnecyChange(Guid id, string frequency);

        Task<Result> CancelUserService(Guid id);

    }
}
