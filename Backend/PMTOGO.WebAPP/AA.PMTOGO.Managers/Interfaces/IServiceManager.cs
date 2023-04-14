using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IServiceManager
    {
        Task<Result> RateUserService(string id, int rate);// rate service
        Task<Result> GetAllServices();//get all services

        Task<Result> GetAllUserServices(string username);//get all user services

        Task<Result> AddServiceRequest(ServiceRequest service, string username);//need to get propertyManager info and address
    }
}
