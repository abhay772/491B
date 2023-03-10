using AA.PMTOGO.Models.Entities;
using System.Text.RegularExpressions;

namespace AA.PMTOGO.Libary
{
    public class InputValidation
    {
        //public Result ValidateEmail(string email)
        //{
        //    var result = new Result();

        //    if (email == null)
        //    {
        //        result.IsSuccessful = false;
        //        result.ErrorMessage = "Invalid email provided. Retry again or contact system administrator";
        //        return result;
        //    }
        //    if (Regex.IsMatch(email, @"^[a-zA-Z0-9-@.\s]+$") && email.Length >= 8 && email.Length < 50 && email.Contains("@"))
        //    {
        //        result.IsSuccessful = true;
        //        return result;
        //    }

        //    result.IsSuccessful = false;
        //    result.ErrorMessage = "Invalid email provided. Retry again or contact system administrator";
        //    return result;
        //}

        public Result ValidateEmail(string email)
        {
            var result = new Result();

            if (string.IsNullOrEmpty(email))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Email address is required.";
                return result;
            }

            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            if (!emailRegex.IsMatch(email))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Email address is not valid.";
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
    }
}
