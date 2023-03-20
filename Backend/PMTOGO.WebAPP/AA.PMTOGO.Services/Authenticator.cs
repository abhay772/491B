using AA.PMTOGO.DAL;
using AA.PMTOGO.Libary;
using AA.PMTOGO.Models;
using AA.PMTOGO.Models.Entities;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

//using System.Text.RegularExpressions;
namespace AA.PMTOGO.Authentication;

public class Authenticator : IAuthenticator
{

    UsersDAO _authNDAO = new UsersDAO();
    InputValidation valid = new InputValidation();


    public async Task<Result> Authenticate(string username, string password)
    {
        Result result = new Result();
        if (valid.ValidateEmail(username).IsSuccessful && valid.ValidatePassphrase(password).IsSuccessful)
        {
            result = await _authNDAO.FindUser(username);

            if (result.IsSuccessful)
            {
                User user = (User)result.Payload!; //null forgiving...FindUser may not return a user paylod object

                string EnteredPassword = EncryptPassword(password, user.Salt);

                if (EnteredPassword.Equals(user.PassDigest))
                {
                    result.IsSuccessful = true;
                    result.Payload = user.Role;
                }

                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Invalid username or password provided. Retry again or contact system admin";
                }
            }

            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid username or password provided. Retry again or contact system admin";
            }
        }

        else
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid username or password provided. Retry again or contact system admin";
        }

        return result;
    }
    public string EncryptPassword(string password, string salt)
    {
        var user_salt = Encoding.UTF8.GetBytes(salt);
        var pass = Encoding.UTF8.GetBytes(password);

        // Lecture Vong 12/13 
        var hash = new Rfc2898DeriveBytes(pass, user_salt, 1000, HashAlgorithmName.SHA512);
        var encryptedPass = hash.GetBytes(64);
        string passDigest = Convert.ToBase64String(encryptedPass);
        return passDigest;
    }

    public async Task<int> GetFailedAttempts(string username)
    {
        Result result = await _authNDAO.FindUser(username);

        if (result.IsSuccessful)
        {
            return await _authNDAO.GetFailedAttempts(username);
        }

        return -1;

    }
     
    public async void ResetFailedAttempts(string username)
    {
        Result result = await _authNDAO.FindUser(username);

        if (result.IsSuccessful)
        {
            await _authNDAO.ResetFailedAttempts(username);
        }

    }

    public async void UpdateFailedAttempts(string username)
    {
        Result result = await _authNDAO.FindUser(username);

        if (result.IsSuccessful)
        {
            await _authNDAO.UpdateFailedAttempts(username);
        }
    }

    public string GenerateOTP()
    {
        string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random rand = new Random();
        string otp = "";
        for (int i = 0; i < 8; i++)
        {
            otp += allowedChars[rand.Next(0, allowedChars.Length)];
        }
        return otp;
    }

    public bool CheckValidOTP(string otp)
    {
        string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        return otp.All(c => allowedChars.Contains(c));
    }


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

        if ((today.Year - dob.Year) >= 13)
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
