using AA.PMTOGO.Models;
using System.Data.SqlClient;
//using System.Data.SqlClient;

namespace AA.PMTOGO.SqlUserDAO
{
    public class UsersDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.UsersDB;Trusted_Connection=True;Encrypt=false";
        //private DatabaseLogger _Logger = new DatabaseLogger("business", new SqlLoggerDAO());
        public UsersDAO()
        {

        }
        //for account authentication // look for the users username/unique ID in sensitive info Table UserAccount
        public User? FindUser(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "select * from UserAccount where Username = @Username";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User();
                        user.username = (string)reader["Username"];
                        user.passDigest = (string)reader["PassDigest"];
                        user.salt = (string)reader["Salt"];
                        //user.isActive = (bool)reader["IsActive"];
                       //user.attempts = (int)reader["Attempts"];

                        return user;
                    }
                }
            }
            return null;
        }

        //look for the email in nonsensitive info Table UserProfile
        public Result DoesUserExist(string email)
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "select * from UserProfile where Email = @Email";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = command.ExecuteReader())
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

        public Result DeactivateUser(string username)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserAccount SET isActive = 0";

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

                string sqlQuery = "UPDATE UserAccount SET isActive = 1";

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

                command.Parameters.AddWithValue("@UserID", userID);
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
        //connection from sensitive to nonsensitive info// can i populate a table as user profile and user account is

    }
}