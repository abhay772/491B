using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;

namespace AA.PMTOGO.DAL
{
    public class UsersDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.UsersDB;Trusted_Connection=True;Encrypt=false";
        private readonly ILogger? _logger;

        //for account authentication // look for the users username/unique ID in sensitive info Table UserAccount
        public Result FindUser(string username)
        {
            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * from UserAccounts WHERE Username = @Username";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = command.ExecuteReader())
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

                            result.IsSuccessful = true;
                            result.Payload = user;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.ErrorMessage = "Account Disabled.";
                        }
                    }
                    catch
                    {

                        result.IsSuccessful = false;
                        result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                        _logger!.Log("FindUser", 4, LogCategory.Server, result);
                    }
                }
            }
            result.IsSuccessful = false;
            result.ErrorMessage = "There was an unexpected server error. Please try again later.";
            return result;
        }

        public Result DeactivateUser(string username)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserAccounts SET IsActive = false WHERE @Username = username";

                var command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Username", username);
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
                    if (e.Number == 208)
                    {
                        result.ErrorMessage = "Specified table not found";
                        _logger!.Log("DeactivateUser", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        public Result ActivateUser(string username)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserAccounts SET IsActive = true WHERE @Username = username";

                var command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Username", username);

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
                    if (e.Number == 208)
                    {
                        result.ErrorMessage = "Specified table not found";
                        _logger!.Log("ActivateUser", 4, LogCategory.DataStore, result);

                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        //sensitive info
        public Result SaveUserAccount(int userID, string passDigest, string salt)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into UserAccounts values (@UserID, @PassDigest, @Salt, @IsActive, @Attempt)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ID", userID);
                command.Parameters.AddWithValue("@PassDigest", passDigest);
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@IsActive", 1);
                command.Parameters.AddWithValue("@Attempt", 0);

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
                    if (e.Number == 208)
                    {
                        result.ErrorMessage = "Specified table not found";
                        _logger!.Log("SaveUserAccount", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        //non-sensitive info
        public Result SaveUserProfile(int userID, string email, string firstName, string lastName, string role)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into UserProfiles values ( @UserID, @Username, @Email, @FirstName, @LastName, @Role)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Id", userID);
                command.Parameters.AddWithValue("@Username", email);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Role", role);


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
                    if (e.Number == 208)
                    {
                        result.ErrorMessage = "Specified table not found";
                        _logger!.Log("SaveUserProfile", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        public void UpdateFailedAttempts(string username)
        {
            //var userAuthenticator = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();


                var command = new SqlCommand("SELECT * FROM UserAccounts WHERE username = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                var reader = command.ExecuteReader();

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


                var command = new SqlCommand("SELECT * FROM UserAccounts WHERE username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);

                var reader = await command.ExecuteReaderAsync();

                reader.Read();
                return (int)reader["Attempts"];
            }
        }

        public void ResetFailedAttempts(string username)
        {
            //var userAuthenticator = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("UPDATE UserAccounts SET Attempts = 0 WHERE @Username = username", connection);
                command.Parameters.AddWithValue("@Usernamae", username);
                command.ExecuteNonQuery();

            }
        }
    }
}
