using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PassDigest { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;

    }
}
