using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services;

public class PropertyEvaluator : IPropertyEvaluator
{
    private readonly IHistoricalSalesDAO _historicalDAO;

    public PropertyEvaluator(IHistoricalSalesDAO historicalDAO)
    {
        _historicalDAO = historicalDAO;
    }

    public async Task<Result> evaluate(PropertyProfile propertyProfile)
    {
        Result result = new Result();

        int[] sales = await _historicalDAO.findSales(propertyProfile);

        if (sales.Length != 0)
        {
            double medianPrice = sales.Average();

            result.IsSuccessful = true;
            result.Payload = medianPrice;

            return result;
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "No data was found";

        return result;
    }
}
