

namespace AA.PMTOGO.Models.Entities;

public class PropertyProfile
{
    public int NoOfBedrooms { get; set; } = 0;
    public int NoOfBathrooms { get; set; } = 0;
    public int SqFeet { get; set; } = 0;
    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}
