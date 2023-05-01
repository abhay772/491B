using System.Security.Claims;


namespace AA.PMTOGO.Models.Entities
{

    public class LoginDTO
    {
        public string? Otp { get; set; }
        public List<Claim>? claims { get; set; }

    }
}