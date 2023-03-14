using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IHistoricalSalesDAO
    {
        Task<int[]> findSales(PropertyProfile propertyProfile);
    }
}