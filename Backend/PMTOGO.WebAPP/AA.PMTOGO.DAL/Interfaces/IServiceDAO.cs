using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL.Interfaces
{
    public interface IServiceDAO
    {

        Task<List<Service>> FindServicesWithQuery(string userQuery, int PageNumber, int PageLimit);
        Task<Result> GetServices();
        Task<Result> FindService(Guid id);
        Task<Result> AddService(Service service);
        Task<Result> DeleteService(Guid id);

    }
}
