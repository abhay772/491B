using Microsoft.Data.SqlClient;
using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.DAO
{
    public class UsersDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.UsersDB;Trusted_Connection=True;Encrypt=false";
        //private DatabaseLogger _Logger = new DatabaseLogger("business", new SqlLoggerDAO());

        //for account authentication // look for the users username/unique ID in sensitive info Table UserAccount
        public Result FindUser(string username)
        {
            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * from UserAccount WHERE Username = @Username";

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

                string sqlQuery = "UPDATE UserAccount SET isActive = false WHERE @Username = username";
          
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
                        // _Logger.AsyncLog("error", "addUser", "Specified table not found.");
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

                string sqlQuery = "UPDATE UserAccount SET isActive = true WHERE @Username = username";

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
                        // _Logger.AsyncLog("error", "addUser", "Specified table not found.");
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        //sensitive info
        public Result SaveUserAccount(string username, string passDigest, string salt)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "insert into UserAccount values (@Username, @PassDigest, @Salt)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PassDigest", passDigest);
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@IsActive", true);

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
                        // _Logger.AsyncLog("error", "addUser", "Specified table not found.");
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

                string sqlQuery = "insert into UserProfile values ( @UserID, @Username, @Email, @FirstName, @LastName, @Role)";

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
                        // _Logger.AsyncLog("error", "addUser", "Specified table not found.");
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

                    command = new SqlCommand("UPDATE UserAccounts SET timestamp = CURRENT_TIMESTAMP", connection);
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


                var command = new SqlCommand("SELECT * FROM UserAccounts WHERE username = @username", connection);
                command.Parameters.AddWithValue("@username", username);

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