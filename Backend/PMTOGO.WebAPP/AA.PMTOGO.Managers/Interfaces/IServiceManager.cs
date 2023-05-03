using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IServiceManager
    {
        Task<Result> RateUserService(string id, int rate, string role);// rate service need role to insert rate in correct column
        Task<Result> GetAllServices();//get all services

        Task<Result> GetAllUserServices(string username, string role);//get all user services

        Task<Result> AddServiceRequest(string id, string frequency, string comments, string username);//need to get propertyManager info and address
        Task<Result> FrequencyChangeRequest(string id, string frequency);
        Task<Result> CancelRequest(string id);
    }
}