using System;

using System.Security.Cryptography;
//using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using AA.PMTOGO.Models;
using AA.PMTOGO.SqlUserDAO;
using System.Diagnostics;

namespace AA.PMTOGO.Registration
{
    public class Registrator
    {
        //private DatabaseLogger _Logger = new DatabaseLogger("business", new SqlLoggerDAO());

        private UsersDAO _UsersDAO = new UsersDAO();

        public Registrator()
        {

        }

        public byte[] GenerateSalt()
        {
            string salt = " ";

            Random rand = new Random();
            salt = (rand.Next(100000, 999999)).ToString();
            Console.WriteLine(salt);

            //DateTime timestamp = DateTime.Now;
            //save OTP and usernmae to dataabase
            //_SqlAuthenticationDAO.A(userName, nonce, timestamp);

            //_SqlLogger.LogData("info", "CreateUserAccount", "Succesfully created an user");
            //userAuthenticator.IsSuccessful = true;
     
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            Console.WriteLine(saltBytes);
            return saltBytes;
        }

        public string EncryptPassword(string password, byte[] salt)
        {

            var pass = Encoding.UTF8.GetBytes(password);
            // Lecture Vong 12/13 
            var hash = new Rfc2898DeriveBytes(pass, salt, 1000, HashAlgorithmName.SHA512);
            var encryptedPass = hash.GetBytes(64);
            string digest = Encoding.UTF8.GetString(encryptedPass);
            return digest;
        }

        public Result CreateUser(int userID, string email, string passDigest, string firstname, string lastname, string role, string salt)
        {
            var result = new Result();
            userID += 1;


            if (!_UsersDAO.DoesUserExist(email).IsSuccessful)
            {
                //add user account
                var timer = Stopwatch.StartNew();
                _UsersDAO.SaveUserAccount(email, passDigest, salt);
                _UsersDAO.SaveUserProfile(userID, email, firstname, lastname, role);
                timer.Stop();
                var seconds = timer.ElapsedMilliseconds / 1000;
                if (seconds > 5)
                {                   
                    result.ErrorMessage = "Took too long";
                    //log it took longer than 5 seconds 
                }
               

                Console.WriteLine("Account created successfully\n");
                Console.WriteLine("Your username is " + email);
                result.IsSuccessful = true;
                return result;
                
            }
            if (_UsersDAO.DoesUserExist(email).IsSuccessful)
            {
                result.ErrorMessage = "User account already exists.";
                result.IsSuccessful = false;
                return result;
            }


            result.ErrorMessage = "Unable to assign username. Retry again or contact system administrator.";
            result.IsSuccessful = false;
            return result;

        }

        //validating user input...

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
}
