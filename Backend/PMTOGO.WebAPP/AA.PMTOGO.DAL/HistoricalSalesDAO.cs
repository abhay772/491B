using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL;

public class HistoricalSalesDAO : IHistoricalSalesDAO
{
    public async Task<int[]> findSales(PropertyProfile propertyProfile)
    {
        // Implement according to the database

        await Task.Delay(10);

        return Array.Empty<int>();
    }
}
