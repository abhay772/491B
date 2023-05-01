using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL.Interfaces
{
    public interface IPriceChartDAO
    {
        Task<Result> GetChartData(int itemID, int time);
        Task<Result> GetItems(int PageNumber, int PageSize);
    }
}