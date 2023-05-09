using AA.PMTOGO.Models.Entities;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using AA.PMTOGO.DAL.Interfaces;
namespace AA.PMTOGO.DAL;

public class UsersDAO : IUsersDAO
{
     private readonly string _connectionString;
     //logging

     public UsersDAO(IConfiguration configuration)
     {
         _connectionString = configuration.GetConnectionString("UsersDbConnectionString")!;
     }
    
    //private string _connectionString = "Server=.\\SQLEXPRESS;Database=AA.UsersDB;Trusted_Connection=True;Encrypt=false";

    //for account authentication // look for the users username/unique ID in sensitive info Table UserAccount
    public async Task<Result> FindUser(string username)
    {
<<<<<<< Updated upstream
        Result result = new Result();
=======
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.UsersDB;Trusted_Connection=True;Encrypt=false";
        //private readonly ILogger? _logger;
>>>>>>> Stashed changes

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT Username, PassDigest, Salt, IsActive, Attempts, Role FROM UserAccounts WHERE Username = @Username";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@Username", username);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                try
                {
                    reader.Read();
                    if ((bool)reader["IsActive"])
                    {
                        User user = new User();
                        user.Username = (string)reader["Username"];
                        user.PassDigest = (string)reader["PassDigest"];
                        user.Salt = (string)reader["Salt"];
                        user.IsActive = (bool)reader["IsActive"];
                        user.Attempt = (int)reader["Attempts"];
                        user.Role = (string)reader["Role"];


                        result.IsSuccessful = true;
                        result.Payload = user;
                        return result;
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "Account Disabled.";
                        return result;
                    }
                }
                catch
                {

                    result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                    result.IsSuccessful = false;

                }
            }
        }
        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid Username or Passphrase. Please try again later.";
        return result;
    }

    public async Task<Result> GetUserAccounts()
    {
        Result result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT UserProfiles.Username, Email, FirstName, LastName, UserProfiles.Role, IsActive FROM UserProfiles INNER JOIN UserAccounts ON UserProfiles.Username = UserAccounts.Username";

            var command = new SqlCommand(query, connection);


            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                try
                {
                    List<User> listOfusers = new List<User>();
                    while (reader.Read())
                    {
                        User user = new User((string)reader["Username"], (string)reader["Email"], (string)reader["FirstName"], (string)reader["LastName"],
                            (string)reader["Role"], (bool)reader["IsActive"]);
                        listOfusers.Add(user);
                        
                    }
                    result.IsSuccessful = true;
                    result.Payload = listOfusers;
                    return result;
                }
                catch
                {

                    result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                    result.IsSuccessful = false;

                }
            }
        }
        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid Username or Passphrase. Please try again later.";
        return result;
    }
    public async Task<Result> GetUser(string username)
    {
        Result result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT Username, Email, FirstName, LastName, Role FROM UserProfiles WHERE Username = @Username";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@Username", username);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                try
                {
                    while (reader.Read())
                    {
                        if (username.Equals(reader["Username"]))
                        {
                            User user = new User();

                            user.Username = (string)reader["Username"];
                            user.Email = (string)reader["Email"];
                            user.FirstName = (string)reader["FirstName"];
                            user.LastName = (string)reader["LastName"];
                            user.Role = (string)reader["Role"];

                            result.IsSuccessful = true;
                            result.Payload = user;
                            return result;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.ErrorMessage = "Account Disabled.";
                            return result;
                        }
                    }
                }
                catch
                {
                    result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                    result.IsSuccessful = false;

<<<<<<< Updated upstream
=======
                        result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                        result.IsSuccessful = false;
                        //_logger!.Log("FindUser", 4, LogCategory.Server, result);
                        
                    }
>>>>>>> Stashed changes
                }
            }
        }
        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid Username or Passphrase. Please try again later.";
        return result;
    }
    public async Task<Result> DoesUserExist(string email)
    {
        var result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT Email FROM UserProfiles WHERE Email = @Email";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@Email", email);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    if (email.Equals(reader["Email"]))
                    {
                        result.IsSuccessful = true;
                        result.ErrorMessage = "User already exists.";
                        return result;
                    }
                }
<<<<<<< Updated upstream
=======

                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        result.ErrorMessage = "Specified table not found";
                        //_logger!.Log("DeactivateUser", 4, LogCategory.DataStore, result);
                    }
                }

>>>>>>> Stashed changes
            }

            result.IsSuccessful = false;
            return result;
        }
<<<<<<< Updated upstream
    }

    public async Task<Result> DeleteUserAccount(string username)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "DELETE FROM UserAccounts WHERE Username = @Username";

            var command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Username", username);
            try
            {
                var rows = await command.ExecuteNonQueryAsync();

                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    return result;
                }

                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "too many rows affected";
                    return result;
                }
            }

            catch (SqlException e)
            {
                if (e.Number == 208)
                {
                    result.ErrorMessage = "Specified table not found";
                }
            }

        }

        result.IsSuccessful = false;
        return result;
    }

    public async Task<Result> DeleteUserProfile(string username)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "DELETE FROM UserProfiles WHERE Username = @Username";

            var command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Username", username);
            try
            {
                var rows = await command.ExecuteNonQueryAsync();

                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    return result;
                }

                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "too many rows affected";
                    return result;
                }
            }

            catch (SqlException e)
            {
                if (e.Number == 208)
                {
                    result.ErrorMessage = "Specified table not found";
                }
            }

        }

        result.IsSuccessful = false;
        return result;
    }

    public async Task<Result> UpdateUserActivation(string username, int active)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "UPDATE UserAccounts SET IsActive = @IsActive WHERE Username = @Username";

            var command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@IsActive", active);

            try
            {
                var rows = await command.ExecuteNonQueryAsync();

                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    return result;
                }

                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "too many rows affected";
                    return result;
                }
            }

            catch (SqlException e)
            {
                if (e.Number == 208)
                {
                    result.ErrorMessage = "Specified table not found";

                }
            }

        }

        result.IsSuccessful = false;
        return result;
    }

    //sensitive info
    public async Task<Result> SaveUserAccount(string username, string passDigest, string salt, string role)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "INSERT into UserAccounts VALUES(@Username, @Role, @PassDigest, @Salt, @IsActive, @Attempts, @Timestamp, @OTP, @OTPTimestamp, @RecoveryRequest)";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Role", role);
            command.Parameters.AddWithValue("@PassDigest", passDigest);
            command.Parameters.AddWithValue("@Salt", salt);
            command.Parameters.AddWithValue("@IsActive", 1);
            command.Parameters.AddWithValue("@Attempts", 0);
            command.Parameters.AddWithValue("@Timestamp", DateTime.Now);
            command.Parameters.AddWithValue("@OTP", "Null");
            command.Parameters.AddWithValue("@OTPTimestamp", DateTime.Now);
            command.Parameters.AddWithValue("@RecoveryRequest", 0);

            try
            {
                var rows = await command.ExecuteNonQueryAsync();

                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    return result;
                }

                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "too many rows affected";
                    return result;
                }
            }

            catch (SqlException e)
            {
                if (e.Number == 208)
                {
                    result.ErrorMessage = "Specified table not found";
                }
            }

        }

        result.IsSuccessful = false;
        return result;
    }
    //non-sensitive info
    public async Task<Result> SaveUserProfile(string email, string firstName, string lastName, string role)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "INSERT into UserProfiles VALUES(@Username, @Email, @FirstName, @LastName, @Role)";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@Username", email);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@Role", role);


            try
            {
                var rows = await command.ExecuteNonQueryAsync();

                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    return result;
                }

                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "too many rows affected";
                    return result;
                }
            }

            catch (SqlException e)
            {
                if (e.Number == 208)
                {
                    result.ErrorMessage = "Specified table not found";
                }
            }

        }

        result.IsSuccessful = false;
        return result;
    }
    public async Task UpdateFailedAttempts(string username)
    {
        //var userAuthenticator = new User();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();


            var command = new SqlCommand("SELECT Username, Attempts FROM UserAccounts WHERE Username = @Username", connection);
            command.Parameters.AddWithValue("@Username", username);

            var reader = await command.ExecuteReaderAsync();

            reader.Read();
            int failedAttempts = (int)reader["Attempts"];

            if (failedAttempts == 0)
            {
                command = new SqlCommand("UPDATE UserAccounts SET Attempts = 1", connection);
                command.ExecuteNonQuery();

                command = new SqlCommand("UPDATE UserAccounts SET Timestamp = CURRENT_TIMESTAMP", connection);
                command.ExecuteNonQuery();
            }
            else if (failedAttempts == 2)
            {
                command = new SqlCommand("UPDATE UserAccounts SET Attempts = 2", connection);
                command.ExecuteNonQuery();
            }
            else
            {
                command = new SqlCommand("UPDATE UserAccounts SET Attempts = 3", connection);
                reader.Close();
                var rows = command.ExecuteNonQuery();
                //TODO: log username, Ip, timestamp to database
            }
            reader.Close();

        }
    }

    public async Task<int> GetFailedAttempts(string username)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();


            var command = new SqlCommand("SELECT Username, Attempts FROM UserAccounts WHERE Username = @Username", connection);
            command.Parameters.AddWithValue("@Username", username);

            var reader = await command.ExecuteReaderAsync();

            int Attempt;
            reader.Read();
            Attempt = (int)reader["Attempts"];
            return Attempt;

        }
    }

    public async Task ResetFailedAttempts(string username)
    {
        //var userAuthenticator = new User();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("UPDATE UserAccounts SET Attempts = 0 WHERE Username = @Username", connection);
            command.Parameters.AddWithValue("@Username", username);
            await command.ExecuteNonQueryAsync();

        }
    }

    public async Task<Result> RequestRecovery(string username)
    {
        Result result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT * FROM UserAccounts WHERE @Username = username";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@Username", username);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                try
                {
                    reader.Read();
                    if ((bool)reader["IsActive"])
                    {
                        User user = new User();
                        user.Username = (string)reader["Username"];
                        user.PassDigest = (string)reader["PassDigest"];
                        user.Salt = (string)reader["Salt"];
                        user.IsActive = (bool)reader["IsActive"];
                        user.Attempt = (int)reader["Attempts"];
                        user.Role = (string)reader["Role"];


                        result.IsSuccessful = true;
                        result.Payload = user;
                        return result;
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "Account Disabled.";
                        return result;
                    }
                }
                catch
                {

                    result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                    result.IsSuccessful = false;
                    //_logger!.Log("FindUser", 4, LogCategory.Server, result);

                }
            }
        }
        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid Username or Passphrase. Please try again later.";
        return result;
    }

    public async Task<Result> ValidateOTP(string username, string otp)
    {
        Result result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("SELECT * FROM UserAccounts WHERE username = @Username AND otp = @OTP", connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@OTP", otp);

            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    DateTime otpTimestamp = (DateTime)reader["OTPTimestamp"];
                    if ((DateTime.Now - otpTimestamp).TotalHours <= 24)
                    {
                        // Update the RecoveryRequest column to 1 if OTP validation is successful
                        // command = new SqlCommand("UPDATE UserAccounts SET RecoveryRequest = 1 WHERE username = @Username", connection);
                        //command.ExecuteNonQuery();

                        result.IsSuccessful = true;
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "OTP is older than 24 hours";
                    }
                }
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid OTP or username";
            }

            reader.Close();
        }
        return result;
    }

    public async Task<Result> UpdatePassword(string username, string passDigest, string salt)
    {
        Result result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("UPDATE UserAccounts SET PassDigest = @PassDigest, Salt = @Salt WHERE Username = @Username", connection);
            command.Parameters.AddWithValue("@PassDigest", passDigest);
            command.Parameters.AddWithValue("@Salt", salt);
            command.Parameters.AddWithValue("@Username", username);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "User not found";
            }
            else
            {
                result.IsSuccessful = true;
            }
        }
        return result;
    }

    public async Task<Result> SaveOTP(string username, string otp)
    {
        Result result = new Result();
        var currentTime = DateTime.Now;
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("UPDATE UserAccounts SET otp = @OTP, OTPTimestamp = @OTPTimestamp WHERE username = @Username", connection);
            command.Parameters.AddWithValue("@OTP", otp);
            command.Parameters.AddWithValue("@OTPTimestamp", currentTime);
            command.Parameters.AddWithValue("@Username", username);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "User not found";
            }
            else
            {
                result.IsSuccessful = true;
            }
        }
        return result;
=======

        public async Task<Result> ActivateUser(string username)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserAccounts SET IsActive = 1 WHERE @Username = username";

                var command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Username", username);

                try
                {
                    var rows = await command.ExecuteNonQueryAsync();

                    if (rows == 1)
                    {
                        result.IsSuccessful = true;
                        return result;
                    }

                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "too many rows affected";
                        return result;
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        result.ErrorMessage = "Specified table not found";
                        //_logger!.Log("ActivateUser", 4, LogCategory.DataStore, result);

                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        //sensitive info
        public async Task<Result> SaveUserAccount(string username, string passDigest, string salt, string role)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into UserAccounts VALUES(@Username, @Role, @PassDigest, @Salt, @IsActive, @Attempts, @Timestamp)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Role", role);
                command.Parameters.AddWithValue("@PassDigest", passDigest);
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@IsActive", 1);
                command.Parameters.AddWithValue("@Attempts", 0);
                command.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                try
                {
                    var rows = await command.ExecuteNonQueryAsync();

                    if (rows == 1)
                    {
                        result.IsSuccessful = true;
                        return result;
                    }

                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "too many rows affected";
                        return result;
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        result.ErrorMessage = "Specified table not found";
                        //_logger!.Log("SaveUserAccount", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        //non-sensitive info
        public async Task<Result> SaveUserProfile(string email, string firstName, string lastName, string role)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into UserProfiles VALUES(@Username, @Email, @FirstName, @LastName, @Role)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Username", email);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Role", role);


                try
                {
                    var rows = await command.ExecuteNonQueryAsync();

                    if (rows == 1)
                    {
                        result.IsSuccessful = true;
                        return result;
                    }

                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "too many rows affected";
                        return result;
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        result.ErrorMessage = "Specified table not found";
                        //_logger!.Log("SaveUserProfile", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        public async Task UpdateFailedAttempts(string username)
        {
            //var userAuthenticator = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();


                var command = new SqlCommand("SELECT * FROM UserAccounts WHERE @Username = username", connection);
                command.Parameters.AddWithValue("@Username", username);

                var reader = await command.ExecuteReaderAsync();

                reader.Read();
                int failedAttempts = (int)reader["Attempts"];

                if (failedAttempts == 0)
                {
                    command = new SqlCommand("UPDATE UserAccounts SET Attempts = 1", connection);
                    command.ExecuteNonQuery();

                    command = new SqlCommand("UPDATE UserAccounts SET Timestamp = CURRENT_TIMESTAMP", connection);
                    command.ExecuteNonQuery();
                }
                else if (failedAttempts == 2)
                {
                    command = new SqlCommand("UPDATE UserAccounts SET Attempts = 2", connection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    command = new SqlCommand("UPDATE UserAccounts SET Attempts = 3", connection);
                    reader.Close();
                    var rows = command.ExecuteNonQuery();
                    //TODO: log username, Ip, timestamp to database

                    //_logger!.Log("UpdateFailedAttempts", 4, LogCategory.DataStore, result);
                }
                reader.Close();

            }
        }

        public async Task<int> GetFailedAttempts(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();


                var command = new SqlCommand("SELECT * FROM UserAccounts WHERE @Username = username", connection);
                command.Parameters.AddWithValue("@Username", username);

                var reader = await command.ExecuteReaderAsync();

                reader.Read();
                return (int)reader["Attempts"];
            }
        }

        public async Task ResetFailedAttempts(string username)
        {
            //var userAuthenticator = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("UPDATE UserAccounts SET Attempts = 0 WHERE @Username = username", connection);
                command.Parameters.AddWithValue("@Usernamae", username);
                await command.ExecuteNonQueryAsync();

            }
        }
>>>>>>> Stashed changes
    }
}
