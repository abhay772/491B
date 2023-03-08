
using System.Security.Principal;

namespace PMTOGO.WebAPP.Models.Entities
{
    public class LoginDTO
    {
        public string? Otp { get; set; }
        public IPrincipal? Principal { get; set; }
    }
}
