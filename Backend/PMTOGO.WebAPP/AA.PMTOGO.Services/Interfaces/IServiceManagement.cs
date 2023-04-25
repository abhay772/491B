using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface IServiceManagement
    {
        Task<Result> GatherServices();

        Task<Result> CreateService(Service service);

        Task<Result> RemoveService(Service service);
    }
}
