using AA.PMTOGO.Models.Entities;
using System.Security.Cryptography;
using System.Text;
using AA.PMTOGO.Libary;
using AA.PMTOGO.DAL;
using AA.PMTOGO.Services.Interfaces;
using AA.PMTOGO.Logging;

namespace AA.PMTOGO.Services
{
    //input validation, error handling , logging
    public class UserManagement : IUserManagement
    {
        UsersDAO _authNDAO = new UsersDAO();
        InputValidation valid = new InputValidation();
        Logger _logger = new Logger();


        //byte[] to string
        public async Task<Result> CreateAccount(string email,string password, string firstname, string lastname, string role)
        {
            Result result = new Result();
            if (valid.ValidateEmail(email).IsSuccessful && valid.ValidatePassphrase(password).IsSuccessful)
            {
                Result result1= new Result();
                result1 = await _authNDAO.FindUser(email);
                if (result1.IsSuccessful == false)//user doesnt exist so procceed
                {
                    //add user account
                    string salt = GenerateSalt();
                    string passDigest = EncryptPassword(password, salt);

                    await _authNDAO.SaveUserAccount(email, passDigest, salt, role);
                    await _authNDAO.SaveUserProfile(email, firstname, lastname, role);

                    //log account created succesfully  
                    await _logger!.Log("CreateAccount", 4, LogCategory.Server, result);

                    User user = new User(email, email, firstname, lastname, role);
                    result.IsSuccessful = true;
                    result.Payload = user;
                    return result;

                }
                else
                {
                    result.ErrorMessage = "User account already exists.";
                    result.IsSuccessful = false;
                    return result;
                }
            }
            else
            {
                result.ErrorMessage = "Invalid email provided.Retry again or contact system administrator";
                result.IsSuccessful = false;
                
            }
            result.IsSuccessful = false;
            return result;
        }

        public async Task<Result> DeleteAccount(string username)
        {
            Result result = new Result();
            if (valid.ValidateEmail(username).IsSuccessful)
            {
                Result result1 = new Result();
                result1 = await _authNDAO.FindUser(username);
                if (result1.IsSuccessful == true)
                {
                    //deactivate user account

                    await _authNDAO.DeleteUserAccount(username);
                    await _authNDAO.DeleteUserProfile(username);
                    //log account deactivate succesfully
                    await _logger!.Log("DeleteAccount", 4, LogCategory.Server, result);
                    result.IsSuccessful = true;
                    return result;

                }
                else
                {
                    result.ErrorMessage = "User account does not exists.";
                    result.IsSuccessful = false;
                    return result;

                }
            }
            else
            {
                result.ErrorMessage = "Unable to delete account. Try again later or contact system administrator.";
                result.IsSuccessful = false;
            }

            return result;
        }
        public string GenerateSalt()
        {
            string salt = "";

            Random rand = new Random();
            salt = rand.Next(100000, 999999).ToString();
            return salt;
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
    }
}
