

namespace AA.PMTOGO.Models.Entities;

public class PropertyProfile
{
    public int NoOfBedrooms { get; set; }
    public int NoOfBathrooms { get; set; }
    public int SqFeet { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }

    public string? Description { get; set; }
}
