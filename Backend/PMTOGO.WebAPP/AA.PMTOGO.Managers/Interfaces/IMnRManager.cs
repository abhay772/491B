using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IMnRManager
    {
        Task<Result> EstimateProject(string username, ProfileChange profileChange);
        Task<Result> FindAllServices(string userQuery, int PageNumber);
    }
}