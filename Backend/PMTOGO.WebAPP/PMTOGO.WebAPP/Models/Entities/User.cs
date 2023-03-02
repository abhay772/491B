using System.ComponentModel.DataAnnotations;

namespace PMTOGO.WebAPP.Models.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PassDigest{ get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
    }
}
