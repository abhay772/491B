using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IPriceChartManager
    {
        Task<Result> GetChartData(int itemID, int time);
        Task<Result> GetItems(int PageNumber);
    }
}