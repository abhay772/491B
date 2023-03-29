using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Diagnostics;

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

        List<double> sales = await _historicalDAO.findSales(propertyProfile);

        if (sales.Count != 0)
        {
            double medianPrice = sales.Average();
            double roundedPrice = Math.Round(medianPrice, 2);
            string formattedPrice = roundedPrice.ToString("N2");


            result.IsSuccessful = true;
            result.Payload = formattedPrice;

            return result;
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "No data was found";

        return result;
    }
}
