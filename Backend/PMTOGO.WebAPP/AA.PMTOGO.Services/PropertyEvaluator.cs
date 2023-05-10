using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Libary;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Services;

public class PropertyEvaluator : IPropertyEvaluator
{
    private readonly IHistoricalSalesDAO _historicalDAO;
    private readonly InputValidation _inputValidation;
    private readonly ISqlPropEvalDAO _sqlPropEvalDAO;


    public PropertyEvaluator(IHistoricalSalesDAO historicalDAO,ISqlPropEvalDAO sqlPropEvalDAO)
    {
        _historicalDAO = historicalDAO;
        _inputValidation = new InputValidation();
        _sqlPropEvalDAO =  sqlPropEvalDAO;
    }

    public async Task<Result> Evaluate(PropertyProfile propertyProfile)
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

    public async Task<Result> LoadProfileAsync(string username)
    {
        bool Validation = _inputValidation.ValidateEmail(username).IsSuccessful;
        if (Validation)
        {
            return await _sqlPropEvalDAO.loadProfileAsync(username);
        }

        Result result = new Result();

        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid Username";

        return result;
    }

    public async Task<Result> SaveProfileAsync(string username, PropertyProfile propertyProfile)
    {
        bool Validation = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidatePropertyProfile(propertyProfile);
        if (Validation)
        {
            return await _sqlPropEvalDAO.saveProfileAsync(username, propertyProfile);
        }

        Result result = new Result();

        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid Username or Property Profile";

        return result;
    }
}
