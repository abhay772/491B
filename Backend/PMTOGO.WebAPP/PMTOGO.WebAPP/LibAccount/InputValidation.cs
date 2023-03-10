using PMTOGO.WebAPP.Models.Entities;
using System.Text.RegularExpressions;

namespace PMTOGO.WebAPP.LibAccount
{
    public class InputValidation
    {
        public Result ValidateEmail(string email)
        {
            var result = new Result();

            if (email == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid email provided. Retry again or contact system administrator";
                return result;
            }
            if (Regex.IsMatch(email, @"^[a-zA-Z0-9-@.\s]+$") && email.Length >= 8 && email.Length < 30 && email.Contains("@"))
            {
                result.IsSuccessful = true;
                return result;
            }

            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid email provided. Retry again or contact system administrator";
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
    }
}
