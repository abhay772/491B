namespace AA.PMTOGO.Models.Entities;

public class UserClaims
{
    public string ClaimUsername { get; set; } = string.Empty;
    public string ClaimRole { get; set; } = string.Empty;

    public UserClaims(string username, string role)
    {
        ClaimUsername = username;
        ClaimRole = role;
    }
}
