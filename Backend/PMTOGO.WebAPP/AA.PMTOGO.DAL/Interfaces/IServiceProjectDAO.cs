using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.DAL.Interfaces;

public interface IServiceProjectDAO
{
    public Result SaveProject(string Username, double EvalChange, double OriginalEval, ProjectDetail projectDetail);
    public Result DeleteProject(string Username, string projectName);
}
