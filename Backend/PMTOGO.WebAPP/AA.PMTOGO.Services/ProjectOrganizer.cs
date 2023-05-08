using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Services;

public class ProjectOrganizer : IProjectOrganizer
{
    private readonly ILogger _logger;
    private readonly IServiceProjectDAO _serviceProjectDAO;

    public ProjectOrganizer(IServiceProjectDAO serviceProjectDAO, ILogger logger)
    {
        _serviceProjectDAO = serviceProjectDAO;
        _logger = logger;
    }

    public async Task<Result> SaveProject(
        string Username, double EvalChange, double OriginalEval, ProjectDetail projectDetail)
    {
        //log 

        Result result = _serviceProjectDAO.SaveProject(Username, EvalChange, OriginalEval, projectDetail);

        return result;
    }    
    public async Task<Result> LoadProjects(string Username)
    {
        //log 

        Result result = _serviceProjectDAO.LoadProjects(Username);

        return result;
    }

    public async Task<Result> DeleteProject(string Username, string projectName)
    {
        //log 

        Result result = _serviceProjectDAO.DeleteProject(Username, projectName);

        return result;
    }
}
