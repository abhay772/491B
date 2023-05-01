using AA.PMTOGO.Libary;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services;
using AA.PMTOGO.Services.Interfaces;
namespace AA.PMTOGO.Managers;

public class ServiceProjectManager : IServiceProjectManager
{
    private readonly IProjectOrganizer _projectOrganizer;
    private readonly InputValidation _inputValidation;

    public ServiceProjectManager(IProjectOrganizer projectOrganizer, InputValidation inputValidation)
    {
        _projectOrganizer = projectOrganizer;
        _inputValidation = inputValidation;
    }

    public async Task<Result> SaveProject(
        string Username, double EvalChange, double OriginalEval, ProjectDetail projectDetail)
    {
        await Task.Delay(5000);
        Result validatonResult = _inputValidation.ValidateProjectDetail(projectDetail);

        if (validatonResult.IsSuccessful) 
        {
            Result result = await _projectOrganizer.SaveProject(Username, EvalChange, OriginalEval, projectDetail);
            return result;
        }

        return validatonResult;
   }

    public async Task<Result> DeleteProject(string Username, string projectName)
    {
        Result validationResult = new Result();
        validationResult.IsSuccessful = _inputValidation.ValidateProjectName(projectName);

        if(validationResult.IsSuccessful){
            Result result = await _projectOrganizer.DeleteProject(Username, projectName);
            return result;
        }

        validationResult.ErrorMessage = "Invalid Project Name";
        return validationResult;
    }
}
