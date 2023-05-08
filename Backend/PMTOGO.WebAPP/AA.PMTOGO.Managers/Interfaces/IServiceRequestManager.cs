using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IServiceRequestManager
    {
        Task<Result> AcceptServiceRequest(string requestId);//update accept
        Task<Result> RemoveServiceRequest(string requestId, string email); // update decline
        Task<Result> GetUserRequests(string username);//get all request for service provider user
    }
}
