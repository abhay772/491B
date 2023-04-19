using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models.Entities
{
    public class EmailInfo
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        public EmailInfo()
        {

        }

        public EmailInfo(string firstName, string lastName, string userEmail, string subject, string description)
        {

            FirstName = firstName;
            LastName = lastName;
            UserEmail = userEmail;
            Subject = subject;
            Description = description;



        }

        public EmailInfo(string userEmail, string subject, string description)
        {

            UserEmail = userEmail;
            Subject = subject;
            Description = description;

        }
    }
}
