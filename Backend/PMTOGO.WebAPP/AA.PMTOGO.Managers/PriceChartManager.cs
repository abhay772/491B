
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Managers;

public class PriceChartManager : IPriceChartManager
{
    private readonly IPriceChartAccessor _priceChartAccessor;

    public PriceChartManager(IPriceChartAccessor priceChartAccessor)
    {
        _priceChartAccessor = priceChartAccessor;
    }

    public async Task<Result> GetItems(int PageNumber)
    {
        int PageSize = 5;

        Result result = await _priceChartAccessor.GetItems(PageNumber, PageSize);

        return result;
    }

    public async Task<Result> GetChartData(int itemID, int time)
    {
        Result result = await _priceChartAccessor.GetChartData(itemID, time);

        return result;
    }
}
