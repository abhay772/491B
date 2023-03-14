

namespace AA.PMTOGO.Models.Entities;

public class PropertyProfile
{
    public int NoOfBedrooms { get; set; }
    public int NoOfBathrooms { get; set; }
    public int SqFeet { get; set; }
    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}
