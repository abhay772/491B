namespace AA.PMTOGO.Models.Entities;

public class ChartData
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public double Price { get; set; }

    public ChartData()
    {

    }
}
