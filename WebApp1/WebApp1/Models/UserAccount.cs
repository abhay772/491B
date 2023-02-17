using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class UserAccount
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string PassDigest { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
    }
}
