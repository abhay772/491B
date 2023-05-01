using AA.PMTOGO.Libary;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AA.PMTOGO.WebAPP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MnRController : ControllerBase
{
    private readonly IServiceProjectManager _serviceProjectManager;
    private readonly IMnRManager _mnRManager;
    private readonly IPriceChartManager _priceChartManager;
    private readonly InputValidation _inputValidation;
    private readonly ClaimValidation _claimValidation;

    public MnRController(IServiceProjectManager serviceProjectManager, IMnRManager mnRManager, IPriceChartManager priceChartManager, ClaimValidation claimValidation, InputValidation inputValidation)
    {
        _serviceProjectManager = serviceProjectManager;
        _mnRManager = mnRManager;
        _priceChartManager = priceChartManager;
        _inputValidation = inputValidation;
        _claimValidation = claimValidation;
    }

    [HttpPut("SaveProject")]
    public async Task<IActionResult> SaveProject([FromBody] SaveProjectDTO saveProjectInput)
    {
        try
        {
            double EvalChange = saveProjectInput.EvalChange;
            double OriginalEval = saveProjectInput.OriginalEval;
            ProjectDetail projectDetail = saveProjectInput.ProjectDetail;


            Result result = new Result();

            Result claimValResult = _claimValidation.ClaimsValidation("Property Manager", Request); 

            if (claimValResult.IsSuccessful == false)
            {
                return Unauthorized();
            }

            string username = ((UserClaims)claimValResult.Payload).ClaimUsername;

            result = await DoWithinTime(() => _serviceProjectManager.SaveProject(username, EvalChange, OriginalEval, projectDetail), 5000);

            if (result.IsSuccessful)
            {
                return Ok("Project Added");
            }


            string errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Unable to Add Project" : result.ErrorMessage;

            return Ok(errorMessage);
        }

        catch (TimeoutException t)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, "Request took longer than 5 seconds and timed out.");
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("DeleteProject")]
    public async Task<IActionResult> DeleteProject(string projectName)
    {
        try
        {
            Result result = new Result();

            Result claimValResult = _claimValidation.ClaimsValidation("Property Manager", Request);

            if (claimValResult.IsSuccessful == false)
            {
                return Unauthorized();
            }

            string username = ((UserClaims)claimValResult.Payload).ClaimUsername;

            result = await DoWithinTime(() => _serviceProjectManager.DeleteProject(username, projectName), 5000);

            if (result.IsSuccessful)
            {
                return Ok("Project Deleted");
            }

            string errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Unable to delete the Project" : result.ErrorMessage;

            return Ok(errorMessage);

        }

        catch (TimeoutException t)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, "Request took longer than 5 seconds and timed out.");
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("LoadServices")]
    public async Task<IActionResult> LoadServices(string userQuery, int PageNumber)
    {
        try
        {
            Result result = await DoWithinTime(() =>
                _mnRManager.FindAllServices(userQuery, PageNumber),
                5000);

            if (result.IsSuccessful)
            {
                return Ok((List<Service>)result.Payload);
            }

            string errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ?  "Unable to find any matching services" : result.ErrorMessage;

            return Ok(errorMessage);

        }
        catch (TimeoutException t)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, "Request took longer than 5 seconds and timed out.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpGet("EstimateProject")]
    public async Task<IActionResult> EstimateProject(int CBedrooms, int CBathrooms, int CSqFeet)
    {

        try
        {
            ProfileChange profileChange = new ProfileChange() { CBedrooms = CBedrooms, CBathrooms = CBathrooms, CSqFeet = CSqFeet };

            Result result = new Result();

            Result claimValResult = _claimValidation.ClaimsValidation("Property Manager", Request);

            if (claimValResult.IsSuccessful == false)
            {
                return Unauthorized();
            }

            string username = ((UserClaims)claimValResult.Payload).ClaimUsername;

            result = await DoWithinTime(() => _mnRManager.EstimateProject(username, profileChange), 5000);
            
            if (result.IsSuccessful)
            {
                return Ok((EstimateDTO)result.Payload);
            }

            string errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Unable to Estimate the Project" : result.ErrorMessage;

            return Ok(errorMessage);
        }

        catch (TimeoutException t)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, "Request took longer than 5 seconds and timed out.");
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpGet("GetPriceChartItems")]
    public async Task<IActionResult> GetItems(int PageNumber)
    {

        try
        {
            Result result = new Result();

            Result claimValResult = _claimValidation.ClaimsValidation("Property Manager", Request);

            result = await DoWithinTime(() => _priceChartManager.GetItems(PageNumber), 5000);

            if (result.IsSuccessful)
            {
                return Ok((List<ChartItems>)result.Payload);
            }

            string errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Unable to Load Chart Items" : result.ErrorMessage;

            return Ok(errorMessage);
        }

        catch (TimeoutException t)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, "Request took longer than 5 seconds and timed out.");
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpGet("GetItemData")]
    public async Task<IActionResult> GetItemData(int ItemID, int time)
    {
        try
        {
            Result claimValResult = _claimValidation.ClaimsValidation("Property Manager", Request);

            Result result = await _priceChartManager.GetChartData(ItemID, time);

            if (result.IsSuccessful)
            {
                return Ok((List<PriceChartDataDTO>)result.Payload);
            }

            string errorMessage = string.IsNullOrEmpty(result.ErrorMessage) ? "Unable to Load Chart Items" : result.ErrorMessage;

            return Ok(errorMessage);
        }

        catch (TimeoutException t)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, "Request took longer than 5 seconds and timed out.");
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private async Task<Result> DoWithinTime(Func<Task<Result>> function, int Time)
    {
        Task<Result> TaskToDO = function();

        Task completedTask = await Task.WhenAny(TaskToDO, Task.Delay(Time));

        if (completedTask != TaskToDO)
        {
            throw new TimeoutException("SaveProject did not complete in 5 seconds");
        }

        Result result = await TaskToDO;
        return result;
    }
}
