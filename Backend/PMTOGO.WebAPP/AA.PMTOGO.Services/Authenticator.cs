
using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Libary;
using AA.PMTOGO.Models.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

//using System.Text.RegularExpressions;
namespace AA.PMTOGO.Services
{
    public class Authenticator : IAuthenticator
    {
        private readonly ILogger? _logger;
        UsersDAO _authNDAO = new UsersDAO();
        InputValidation valid = new InputValidation();


        public Result Authenticate(string username, string password)
        {
            Result result = new Result();
            if (valid.ValidateEmail(username).IsSuccessful && valid.ValidatePassphrase(password).IsSuccessful)
            {
                result = _authNDAO.FindUser(username);

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


        public string GenerateSalt()
        {
            string salt = " ";

            Random rand = new Random();
            salt = rand.Next(100000, 999999).ToString();
            return salt;
        }

        public string EncryptPassword(string password, string salt)
        {
            var user_salt = Encoding.UTF8.GetBytes(salt);
            var pass = Encoding.UTF8.GetBytes(password);
            Console.WriteLine(pass);

            // Lecture Vong 12/13 
            var hash = new Rfc2898DeriveBytes(pass, user_salt, 1000, HashAlgorithmName.SHA512);
            var encryptedPass = hash.GetBytes(64);
            string passDigest = Encoding.UTF8.GetString(encryptedPass);
            return passDigest;
        }

        public async Task<int> GetFailedAttempts(string username)
        {
            return await _authNDAO.GetFailedAttempts(username);
        }

        public void ResetFailedAttempts(string username)
        {
            _authNDAO.ResetFailedAttempts(username);
        }

        public void UpdateFailedAttempts(string username)
        {
            _authNDAO.UpdateFailedAttempts(username);
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

    }
}
