using System.Security.Claims;
using System.Security.Principal;


namespace AA.PMTOGO.Models.Entities
{

    public class LoginDTO
    {
        public string? Otp { get; set; }
        public List<Claim>? claims { get; set; }

    }
}