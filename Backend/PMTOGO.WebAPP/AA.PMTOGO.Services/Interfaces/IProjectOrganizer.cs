using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface IProjectOrganizer
    {
        Task<Result> SaveProject(string Username, double EvalChange, double OriginalEval, ProjectDetail projectDetail);
        Task<Result> LoadProjects(string Username);
        Task<Result> DeleteProject(string Username, string projectName);
    }
}