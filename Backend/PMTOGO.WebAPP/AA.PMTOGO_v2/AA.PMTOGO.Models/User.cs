using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models
{
    public class User
    {
       

        public string Username { get; set; }

        public byte[] Password { get; set; }

        public byte[] salt { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string role { get; set; }

        public bool isActive { get; set; }

        public DateTime dob { get; set; }

        public User()
        {

        }

        public User(string username, byte[] password, byte[] salt, string firstName, string lastName, string role, DateTime dob)
        {
            Username = username;
            Password = password;
            this.salt = salt;
            this.firstName = firstName;
            this.lastName = lastName;
            this.role = role;
            this.dob = dob;
        }
        
    }
}
