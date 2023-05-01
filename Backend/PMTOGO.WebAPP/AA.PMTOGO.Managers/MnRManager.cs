using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Libary;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Managers;

public class MnRManager : IMnRManager
{

    private readonly IServiceFinder _serviceFinder;
    private readonly IPropertyEvaluator _propertyEvaluator;
    private readonly InputValidation _inputValidation;

    public MnRManager(IServiceFinder serviceFinder, IPropertyEvaluator propertyEvaluator)
    {
        _serviceFinder = serviceFinder;
        _propertyEvaluator = propertyEvaluator;
        _inputValidation = new InputValidation();
    }

    public async Task<Result> FindAllServices(string userQuery, int PageNumber)
    {
        Result result = new Result();
        int PageLimit = 5;

        if (userQuery.Length > 0 && userQuery.Length <= 50)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid Query size";

            return result;
        }

        List<Service> services = await _serviceFinder.FindAllServices(userQuery, PageNumber, PageLimit);

        if (services.Count > 0)
        {
            foreach (Service service in services)
            {
                if (service.ServiceDescription.Length > 250)
                {
                    service.ServiceDescription = service.ServiceDescription.Substring(0, 250);
                }
            }

            result.IsSuccessful = true;
            result.Payload = services.Take(PageLimit).ToList();
            return result;
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "No services found";
        return result;
    }

    public async Task<Result> EstimateProject(string username, ProfileChange profileChange)
    {
        Result LoadProfileResult = await _propertyEvaluator.LoadProfileAsync(username);

        Result result = new Result();

        if (LoadProfileResult.IsSuccessful == false)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "No Profile Found";
        }

        PropertyProfile propertyProfile = (PropertyProfile)LoadProfileResult.Payload;

        Result evaluationResult = await _propertyEvaluator.Evaluate(propertyProfile);

        if(evaluationResult.IsSuccessful == false)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Unable to find a Property Profile";
            return result;
        }

        double OEval = double.Parse(evaluationResult.Payload.ToString());

        if (_inputValidation.ValidateChangeInProfile(profileChange, propertyProfile) == false)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid Profile Change";
        }

        PropertyProfile CPropertyProfile = propertyProfile;


        CPropertyProfile.NoOfBedrooms = propertyProfile.NoOfBedrooms - profileChange.CBedrooms;
        CPropertyProfile.NoOfBathrooms = propertyProfile.NoOfBathrooms - profileChange.CBathrooms;
        CPropertyProfile.SqFeet = propertyProfile.SqFeet - profileChange.CSqFeet;

        Result CEvaluationResult = await _propertyEvaluator.Evaluate(CPropertyProfile);

        Double CEval = double.Parse(CEvaluationResult.Payload.ToString());

        double EvalChange = OEval - CEval;

        EstimateDTO estimateDTO = new EstimateDTO() { OEval = OEval, EvalChange = EvalChange };

        result.IsSuccessful = true;
        result.Payload = estimateDTO;
        return result;
    }
}
