using AA.PMTOGO.Models.Entities;
using System.Text.RegularExpressions;

namespace AA.PMTOGO.Libary
{
    public class InputValidation
    {
        public Result ValidateEmail(string email)
        {
            var result = new Result();

            // Check if the email string is null or empty
            if (string.IsNullOrEmpty(email))
            {

                result.IsSuccessful = false;
                result.ErrorMessage = "Email address is not valid";
                return result;
            }

            // Split the email address into local and domain parts
            string[] parts = email.Split('@');
            if (parts.Length != 2)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Email address is not valid";
                return result;
            }

            // Check if the local and domain parts are not empty
            if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Email address is not valid";
                return result;
            }

            // Split the domain into name and extension
            string[] domainParts = parts[1].Split('.');
            if (domainParts.Length < 2)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Email address is not valid";
                return result;
            }

            // Check if the name and extension are not empty
            if (string.IsNullOrEmpty(domainParts[0]) || string.IsNullOrEmpty(domainParts[1]))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Email address is not valid";
                return result;
            }

            // Check if the email is in the expected format
            if (email.IndexOf('@') != email.LastIndexOf('@') ||
                email.IndexOf('@') < 1 ||
                email.LastIndexOf('.') < email.IndexOf('@') + 2)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Email address is not valid";
                return result;
            }

            result.IsSuccessful = true;
            return result;
        }


        public Result ValidatePassphrase(string passWord)
        {
            var result = new Result();
            if (passWord == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid passphrase provided. Retry again or contact system administrator";
                return result;
            }
            if (Regex.IsMatch(passWord, @"[a-zA-Z0-9-.,@!\s]") && passWord.Length >= 8)
            {
                result.IsSuccessful = true;
                return result;
            }
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid passphrase provided. Retry again or contact system administrator";
            return result;
        }

        public Result ValidateUsername(string name)
        {
            var result = new Result();
            if (name == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid";
                return result;
            }
            if (Regex.IsMatch(name, @"^[a-z0-9-@.\s]+$") && name.Length >= 8)
            {
                result.IsSuccessful = true;
                return result;
            }
            result.IsSuccessful = false;
            result.ErrorMessage = "Cannot use Special Characters";
            return result;
        }
        public Result ValidateDateOfBirth(DateTime dob)
        {
            var result = new Result();

            var today = DateTime.Now;

            if (today.Year - dob.Year >= 13)
            {
                result.IsSuccessful = true;
                return result;
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid age. User must be 13 years old or older.";
                return result;
            }
        }

        public Result ValidateRole(string role)
        {
            var result = new Result();

            var today = DateTime.Now;

            if (role == "Property Manager" || role == "Service Provider")
            {
                result.IsSuccessful = true;
                return result;
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid Role.";
                return result;
            }
        }

        public bool ValidatePropertyProfile(PropertyProfile propertyProfile)
        {
            bool isValid = (propertyProfile.NoOfBedrooms != 0) && 
                (propertyProfile.NoOfBathrooms != 0) && 
                (propertyProfile.SqFeet != 0) && 
                (propertyProfile.Address1 != string.Empty) && 
                (propertyProfile.Address2 != string.Empty) && 
                (propertyProfile.City != string.Empty) &&     
                (propertyProfile.State != string.Empty) &&
                           (propertyProfile.Zip != string.Empty);
            return isValid;
        }
    }
}
