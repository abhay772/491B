namespace AA.PMTOGO.Models.Entities;

public class SaveProjectDTO
{
    public double EvalChange { get; set; }
    public double OriginalEval { get; set; }
    public ProjectDetail ProjectDetail { get; set; }

    public SaveProjectDTO()
    {

    }
}