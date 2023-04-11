using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL
{
    public interface IHistoricalSalesDAO
    {
        Task<List<double>> findSales(PropertyProfile propertyProfile);
    }
}