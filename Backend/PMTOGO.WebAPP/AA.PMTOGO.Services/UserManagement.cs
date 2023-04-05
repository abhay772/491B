using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Infrastructure.Interfaces;
using System.Security.Cryptography;
using System.Text;
using AA.PMTOGO.Libary;
using AA.PMTOGO.DAL;
using System.Net.Mail;
using System.Net;

namespace AA.PMTOGO.Services
{
    public class UserManagement : IUserManagement
    {
        //private readonly ILogger? _logger;
        UsersDAO _authNDAO = new UsersDAO();
        InputValidation valid = new InputValidation();
        private readonly ILogger? _logger;


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
        //Variables
        private string _OTP;
        private List<User> _users = new List<User>();
        public async Task<Result> RecoveryService(string email)
        {
            Result result = new Result();
            var mailAddress = new System.Net.Mail.MailAddress(email);
            if (mailAddress.Address == email)
            {
                UsersDAO usersDAO = new UsersDAO();

                if (usersDAO.DoesUserExist(email).Result.IsSuccessful)
                {
                    string otp = GenerateOTP();
                    return AutomaticEmail(email, otp).Result;
                }
            }
            return result;
        }

        public string GenerateOTP()
        {
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.-@";
            Random rand = new Random();
            for (int i = 0; i < 8; i++)
            {
                _OTP += allowedChars[rand.Next(0, allowedChars.Length)];
            }

            return _OTP;
        }

        public async Task<bool> ValidateOTP(string email, string otp)
        {
            UsersDAO usersDAO = new UsersDAO();
            if (otp == _OTP)
            {
                usersDAO.SetSuccessfulOTP(email);
                return true;
            }
            return false;
        }

        public async Task<Result> AutomaticEmail(string email, string OTP)
        {
            // Email configuration
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string senderEmail = "test@gmail.com";
            string senderPassword = "new one later";
            string recipientEmail = "later.com";
            string subject = "Account Recovey - One Time Password";
            string body = OTP;
            Result result = new Result();
            // Create SMTP client and login
            var smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            // Create email message
            var message = new MailMessage(senderEmail, recipientEmail, subject, body);

            try
            {
                // Send email
                smtpClient.Send(message);
                Console.WriteLine("Email sent successfully.");
                result.IsSuccessful = true;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
            finally
            {
                // Cleanup
                message.Dispose();
                smtpClient.Dispose();
            }
            result.IsSuccessful = false;
            return result;
        }
        
        public async Task<List<User>> GetRecoveryRequests()
        {
            UsersDAO usersDAO = new UsersDAO();
            _users = usersDAO.getRecoveryRequests();
            return _users;
        }

        public async Task<bool> AccountResponse(Boolean response, string email)
        {
            UsersDAO userDAO = new UsersDAO();
            if (response == true)
            {
                var activateResult = userDAO.ActivateUser(email);
                if (activateResult.Result.IsSuccessful)
                {
                    return true;
                }
            }
            else if (response == false)
            {
                var rejectResult = userDAO.RejectUser(email);
                if (rejectResult.IsSuccessful)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
