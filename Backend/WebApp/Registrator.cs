using System;
using AA.PMTOGO.Models;
using AA.PMTOGO.Registration;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace AA.PMTOGO.Registration
{
    public class Registrator
    {
        //private DatabaseLogger _Logger = new DatabaseLogger("business", new SqlLoggerDAO());
    /*  
        private SqlRegistrationDAO _SqlRegistrationDAO = new SqlRegistrationDAO();

        public Registrator()
        {

        }

        public byte[] GenerateSalt()
        {
            string salt = " ";

            Random rand = new Random();
            salt = (rand.Next(100000, 999999)).ToString();
            System.Console.WriteLine(salt);

            //DateTime timestamp = DateTime.Now;
            //save OTP and usernmae to dataabase
            //_SqlAuthenticationDAO.A(userName, nonce, timestamp);
            //_SqlLogger.LogData("info", "CreateUserAccount", "Succesfully created an user");
            //userAuthenticator.IsSuccessful = true;
            // var saltBytes = Encoding.UTF8.GetBytes(salt);
            var saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);
            System.Console.WriteLine(saltBytes);
            return saltBytes;
        }

        public byte[] EncrpytPassword(string password, byte[] salt)
        {

            var pass = Encoding.UTF8.GetBytes(password);
            // Lecture Vong 12/13 
            var hash = new Rfc2898DeriveBytes(pass, salt, 1000, HashAlgorithmName.SHA512);
            var encryptedPass = hash.GetBytes(64);

            return encryptedPass;
        }

        public Result CreateUser(string email, byte[] password, string firstname, string lastname, DateTime dob, int role, byte[] salt)
        {
            var result = new Result();
            if (!_SqlRegistrationDAO.FindUser(email).IsSuccessful)
            {
                //add user account
                _SqlRegistrationDAO.SaveUserAccount(email, password, salt);
                //_Logger.Log("info", "CreateUserAccount", "Succesfully created an user");

                //add user profile
                _SqlRegistrationDAO.SaveUserProfile(email, firstname, lastname, dob, role);
                //_Logger.Log("info", "CreateUserProfile", "Succesfully created an user");
                result.IsSuccessful = true;
                return result;
            }
            if (_SqlRegistrationDAO.FindUser(email).IsSuccessful)
            {
                result.ErrorMessage = "User account already exists.";
                result.IsSuccessful = false;
                return result;
            }


            result.ErrorMessage = "Unable to assign username. Retry again or contact system administrator.";
            result.IsSuccessful = false;
            return result;
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
            if (Regex.IsMatch(email, @"^[a-zA-Z0-9-@.\s]+$") && email.Length >= 8 && email.Length < 30)
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
            if (Regex.IsMatch(passWord, @"^[a-zA-Z0-9-@.\s]+$") && passWord.Length >= 8 && passWord.Length < 30)
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
            if (Regex.IsMatch(name, @"^[a-zA-Z0-9-\s]+$") && name.Length >= 8 && name.Length < 30)
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
            if ((DateTime.Now - dob).Ticks >= 13)
            {
                result.IsSuccessful = true;
                return result;
            }
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid age. User must be 13 years old or older.";
            return result;
        }
    */
    }
}
