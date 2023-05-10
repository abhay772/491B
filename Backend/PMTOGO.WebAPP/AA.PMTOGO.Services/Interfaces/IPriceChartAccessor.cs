using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface IPriceChartAccessor
    {
        Task<Result> GetChartData(int itemID, int time);
        Task<Result> GetItems(int PageNumber, int PageSizez);
    }
}