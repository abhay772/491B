namespace AA.PMTOGO.Models.Entities;

public class ProjectDetail
{
    public string ProjectName { get; set; } = string.Empty;
    public List<int>? ServiceIDs { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly  EndDate { get; set; }
    public TimeOnly ServiceTime { get; set; }
    public double Budget { get; set; } = 0;
}
 