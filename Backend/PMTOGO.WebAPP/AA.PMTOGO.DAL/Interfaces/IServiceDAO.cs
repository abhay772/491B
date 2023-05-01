using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL.Interfaces
{
    public interface IServiceDAO
    {
        Task<Result> AddService(string serviceName, string serviceType, string serviceDescription, string serviceProviderEmail, string serviceProvider);
        Task<Result> DeleteService(string serviceName, string serviceType, string serviceProviderEmail);
        Task<Result> FindService(string serviceName, string serviceProviderEmail, string serviceType);
        Task<List<Service>> FindServicesWithQuery(string userQuery, int PageNumber, int PageLimit);
        Task<Result> GetServices();
    }
}