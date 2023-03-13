using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Infrastructure.Interfaces;
using System.Security.Cryptography;
using System.Text;
using AA.PMTOGO.Libary;
using AA.PMTOGO.DAL;

namespace AA.PMTOGO.Services
{
    public class UserManagement : IUserManagement
    {
        //private readonly ILogger? _logger;
        UsersDAO _authNDAO = new UsersDAO();
        InputValidation valid = new InputValidation();

        //static int userID = 0;
                                            //byte[] to string
        public async Task<Result> CreateAccount(string email,string password, string firstname, string lastname, string role)
        {
            Result result = new Result();
            if (valid.ValidateEmail(email).IsSuccessful && valid.ValidatePassphrase(password).IsSuccessful)
            {
                Result result1= new Result();
                result1 = await _authNDAO.DoesUserExist(email);
                if (result1.IsSuccessful == false)//user doesnt exist so procceed
                {
                    Console.WriteLine("User doesnt exist procceed");
                    //add user account
                    //userID += 1;
                    string salt = GenerateSalt();
                    string passDigest = EncryptPassword(password, salt);

                    await _authNDAO.SaveUserAccount(email, passDigest, salt);
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

        public async Task<Result> AddServiceToUser(string username, Service service)
        {
            var result = new Result();
            var user = (await _authNDAO.FindUser(username)).Payload as User;

            if (user is null)
            {
                result.IsSuccessful|= false;
                result.Payload = null;
                result.ErrorMessage = "User cannot be found.";

                return result;
            }
            else
            {
                user.Services.Add(service);

                result.IsSuccessful = true;
                result.Payload = user;
                result.ErrorMessage = null;

                return result;
            }
        }

        public async Task<Result> DeactivateAccount(string username, string password)
        {
            Result result = new Result();
            if (valid.ValidateEmail(username).IsSuccessful && valid.ValidatePassphrase(password).IsSuccessful)
            {
                Result result1 = new Result();
                result1 = await _authNDAO.FindUser(username);
                if (result1.IsSuccessful == false)
                {
                    //deactivate user account

                   await _authNDAO.DeactivateUser(username);

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
                result.ErrorMessage = "Unable to delete account. Retry again or contact system administrator.";
                result.IsSuccessful = false;
            }

            return result;
        }
        public string GenerateSalt()
        {
            string salt = "";

            Random rand = new Random();
            salt = rand.Next(100000, 999999).ToString();
            Console.WriteLine(salt);
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
            //Console.WriteLine(passDigest);
            return passDigest;
        }
    }
}
