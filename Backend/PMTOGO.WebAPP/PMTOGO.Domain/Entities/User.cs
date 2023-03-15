using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AA.PMTOGO.Models.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PassDigest { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int Attempt { get; set; }

        public User()
        {

        }

        public User(string username, string email, string firstName, string lastName, string role)
        {

            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            IsActive = true;
            Attempt = 0;

        }
        public User(string username, string role, string passDigest, string salt, bool isActive, int attempt)
        {
            Username = username;
            Role = role;
            PassDigest = passDigest;
            Salt = salt;
            IsActive = isActive;
            Attempt = attempt;
        }
    }
}
