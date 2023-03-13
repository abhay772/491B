using AA.PMTOGO.Models.Entities;


namespace AA.PMTOGO.Managers;

public class PropEvalManager : IPropEvalManager
{
    private readonly ISqlPropEvalDAO _sqlPropEvalDAO;
    private readonly IPropertyEvaluator _evaluator;

    public PropEvalManager(ISqlPropEvalDAO sqlPropEvalDAO, IPropertyEvaluator evaluator)
    {
        _sqlPropEvalDAO = sqlPropEvalDAO;
        _evaluator = evaluator;
    }

    public async Task<Result> loadProfileAsync(string username)
    {
        return await _sqlPropEvalDAO.LoadProfileAsync(username);
    }

    public async Task<Result> saveProfileAsync(string username, PropertyProfile propertyProfile)
    {
        return await _sqlPropEvalDAO.SaveProfileAsync(username, propertyProfile);
    }

    public async Task<Result> evaluateAsync(string username, PropertyProfile propertyProfile)
    {
        Result result = new Result();

        try
        {
            Result evaluationResult = await _evaluator.evaluate(username, propertyProfile);

            if (result.IsSuccessful)
            {
                Result saveEvalResult = await _sqlPropEvalDAO.updatePropEval(username, (int)evaluationResult.Payload);
            }

            return evaluationResult;
        }

        catch (Exception ex)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = ex.Message;
        }
        return await _sqlPropEvalDAO._evaluateAsync(username, propertyProfile);
    }
}
