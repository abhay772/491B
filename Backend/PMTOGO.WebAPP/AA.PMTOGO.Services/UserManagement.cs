using AA.PMTOGO.Models.Entities;
using System.Security.Cryptography;
using System.Text;
using AA.PMTOGO.Libary;
using AA.PMTOGO.DAL;
using System.Net.Mail;
using System.Net;
using AA.PMTOGO.Services.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.DAL.Interfaces;

namespace AA.PMTOGO.Services
{
    //input validation, error handling , logging
    public class UserManagement : IUserManagement
    {
        IUsersDAO _authNDAO;

        InputValidation valid = new InputValidation();
        private readonly ILogger? _logger;

        public UserManagement(ILogger logger, IUsersDAO usersDAO)
        {
            _logger = logger;
            _authNDAO = usersDAO;
        }

        public async Task<Result> GatherUsers()
        {
            Result result = new Result();
            try
            {
                result = await _authNDAO.GetUserAccounts();
                await _logger!.Log("GetUserAccounts", 4, LogCategory.Business, result);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Get User Accounts Unsuccessful. Try Again Later";
                await _logger!.Log("GetUserAccounts", 4, LogCategory.Business, result);
            }
            return result;
        }
        public async Task<User?> GetUser(string username)
        {
            Result result = new();

            if (valid.ValidateEmail(username).IsSuccessful)
            {
                Result origin = await _authNDAO.FindUser(username);

                if (origin.IsSuccessful)
                {
                    return origin.Payload as User;
                }
                else return null;
            }
            else
            {
                return null;
            }
        }

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

        public async Task<Result> DisableAccount(string username, int active)
        {
            Result result = new Result();
            if (valid.ValidateEmail(username).IsSuccessful)
            {
                Result result1 = new Result();
                result1 = await _authNDAO.FindUser(username);
                if (result1.IsSuccessful == true)
                {
                    //deactivate user account

                    await _authNDAO.UpdateUserActivation(username, active);

                    //log account deactivate succesfully
                    await _logger!.Log("DisableAccount", 4, LogCategory.Server, result);
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
                result.ErrorMessage = "Unable to disable account. Try again later.";
                result.IsSuccessful = false;
            }

            return result;
        }

        public async Task<Result> EnableAccount(string username, int active)
        {
            Result result = new Result();
            if (valid.ValidateEmail(username).IsSuccessful)
            {
                Result result1 = new Result();
                result1 = await _authNDAO.GetUser(username);
                if (result1.IsSuccessful == true)
                {
                    //deactivate user account

                    await _authNDAO.UpdateUserActivation(username,active);
                    
                    //log account activate succesfully
                    await _logger!.Log("EnableAccount", 4, LogCategory.Server, result);
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
                result.ErrorMessage = "Unable to enable account. Try again later";
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

        public async Task<Result> AccountRecovery(string email)
        {
            Result result = new Result();
            result = await _authNDAO.FindUser(email);
            
            if (result.IsSuccessful) 
            {
                await EmailOTP(email);
            }
            return result;
        }
        public async Task<bool> EmailOTP(string userEmail)
        {
            string companyEmail = "DemonicKhmer@gmail.com";
            string companyEmailKey = "Your Email third party application key. 2 factor authN must be activated for the gmail account";
            string emailSubject = "Account Recovery - OTP";
            string emailBody = "Your One-Time Password is : ";
            var otp = "";

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(companyEmail, companyEmailKey);
            smtpClient.EnableSsl = true;

            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.-@";
            Random rand = new Random();
            for (int i = 0; i < 8; i++)
            {
                otp += allowedChars[rand.Next(0, allowedChars.Length)];
                emailBody += allowedChars[rand.Next(0, allowedChars.Length)];
            }

            var message = new MailMessage(companyEmail, userEmail, emailSubject, emailBody);

            return false;

           /* // delete
            Console.WriteLine(otp);
            await _authNDAO.SaveOTP(userEmail, otp);
            return true;
            //delete
            try
            {
                await _authNDAO.SaveOTP(userEmail, otp);
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email");
            }
            return false;*/
            
        }
    }
}

