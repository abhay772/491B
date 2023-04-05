using AA.PMTOGO.Models.Entities;
using System.Data;
using System.Data.SqlClient;
namespace AA.PMTOGO.DAL;

public class UsersDAO
{
    private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.UsersDB;Trusted_Connection=True;Encrypt=false";
    //private readonly ILogger? _logger;

    //for account authentication // look for the users username/unique ID in sensitive info Table UserAccount
    public async Task<Result> FindUser(string username)
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
    public async Task<Result> GetUser(string username)
    {
        Result result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT * FROM UserProfile WHERE @Username = username";

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
                    //_logger!.Log("FindUser", 4, LogCategory.Server, result);

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

            string sqlQuery = "SELECT * FROM UserProfiles WHERE @Email = email";

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
            }

            result.IsSuccessful = false;
            return result;
        }
    }

    public async Task<Result> DeleteUserAccount(string username)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "DELETE FROM UserAccounts WHERE @Username = username";

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
                    //_logger!.Log("DeactivateUser", 4, LogCategory.DataStore, result);
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

            string sqlQuery = "DELETE FROM UserProfiles WHERE @Username = username";

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
                    //_logger!.Log("DeactivateUser", 4, LogCategory.DataStore, result);
                }
            }

        }

        result.IsSuccessful = false;
        return result;
    }

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
            command.Parameters.AddWithValue("@Username", username);
            await command.ExecuteNonQueryAsync();

        }
    }
    public Result RejectUser(string email)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "UPDATE UserAccounts SET isActive = 0";

            var command = new SqlCommand(sqlQuery, connection);

            try
            {
                var rows = command.ExecuteNonQuery();

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
            
            }

        }

        result.IsSuccessful = false;
        return result;
    }
    public List<User> getRecoveryRequests()
    {
        List<User> users = new List<User>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT * FROM RecoveryRequests WHERE RecoveryRequest = 1", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User();
                        user.Username = (string)reader["Email"];
                        user.RecoveryRequest = (bool)reader["RecoveryRequest"];
                        user.IsActive = (bool)reader["IsActive"];
                        user.SuccessfulOTP = (bool)reader["SuccessfulOTP"];
                        users.Add(user);
                    }
                }
            }
        }
        return users;
    }
    public Result SetSuccessfulOTP(string email)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "UPDATE RecoveryRequests SET successfulOTP = 1";

            var command = new SqlCommand(sqlQuery, connection);

            try
            {
                var rows = command.ExecuteNonQuery();

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
             
            }

        }

        result.IsSuccessful = false;
        return result;
    }


}
