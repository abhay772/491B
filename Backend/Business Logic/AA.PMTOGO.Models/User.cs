using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models
{
    public class User
    {
        
        public int userID { get; set; } = 0;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string passDigest { get; set; } = string.Empty;
        public string salt { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty; //unique id /username

        public User() { }

        //user profile non sensitive info
        public User(string username, string firstName, string lastName, string email, string role)
        {
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.role = role;
        }
        // user account sensitive info for logging
        public User(int userID, string username)
        { 
            this.userID = userID;
            this.username = username;
        }  


        //user account sensitive info
        public User(string username, string salt, string passDigest)
        { 
            this.username = username;
            this.salt = salt;
            this.passDigest = passDigest;
        }
    }
}
