using System.Security.Principal;


namespace AA.PMTOGO.Models;

public class LoginDTO
{
    public string otp { get; set; }
    public IPrincipal principal { get; set; }

}
