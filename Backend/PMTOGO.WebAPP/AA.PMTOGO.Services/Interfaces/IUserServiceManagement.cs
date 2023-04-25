using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Services.Interfaces
{
    public interface IUserServiceManagement
    {
        Task<Result> Rate(Guid id, int rate, string role);
        Task<Result> CreateRequest(Guid id, string type, string frequency);
        Task<Result> AddRequest(Guid id, string frequency, string comments, string username);
        Task<Result> GatherUserServices(string username, string role);

        bool CheckRate(int rate);
    }
}
