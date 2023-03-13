using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
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

        public User(int id, string username, string email, string firstName, string lastName, string role)
        {
            Id = id;
            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;

        }
        public User(int id, string passDigest, string salt, bool isActive, int attempt)
        {
            Id = id;
            PassDigest = passDigest;
            Salt = salt;
            IsActive = isActive;
            Attempt = attempt;
        }
    }
}
