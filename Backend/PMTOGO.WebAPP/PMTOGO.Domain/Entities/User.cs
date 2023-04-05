using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AA.PMTOGO.Models.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PassDigest { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public bool RecoveryRequest { get; set; }
        public bool IsActive { get; set; }
        public bool SuccessfulOTP { get; set; }
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
        public User(string username, bool recoveryRequest, bool isActive, bool successfulOTP)
        {
            Username = username;
            RecoveryRequest = recoveryRequest;
            IsActive = isActive;
            SuccessfulOTP = successfulOTP;
        }
    }
}
