using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface IServiceProjectManager
    {
        Task<Result> SaveProject(string Username, double EvalChange, double OriginalEval, ProjectDetail projectDetail);
        Task<Result> DeleteProject(string Username, string projectName);
        Task<Result> LoadProjects(string Username);
    }
}