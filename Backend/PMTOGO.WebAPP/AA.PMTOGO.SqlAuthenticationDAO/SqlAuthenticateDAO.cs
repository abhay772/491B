using AA.PMTOGO.Models;
using System.Data.SqlClient;

namespace AA.PMTOGO.SqlAuthenticationDAO;

public class SqlAuthenticateDAO
{
    private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.Users;Trusted_Connection=True;Encrypt=false";

    public Result FindUser(string username)
    {
        Result result = new Result();
        var userAuthenticator = new User();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            //User database should have username/ password/ attempts
            var command = new SqlCommand("SELECT * FROM UserAccounts WHERE username = @username", connection);
            command.Parameters.AddWithValue("@username", username);

            using (var reader = command.ExecuteReader())
            {
                Result result1 = new Result();

                reader.Read();

                try
                {
                    reader.Read();

                    if ((bool)reader["IsActive"])
                    {
                        User user = new User();

                        user.Username = (string)reader["Username"];
                        user.Password = (byte[])reader["Password"];
                        user.salt = (byte[])reader["Salt"];
                        user.role = (string)reader["Role"];

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

    public void UpdateFailedAttempts(string username)
    {
        var userAuthenticator = new User();
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
        var userAuthenticator = new User();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();


            var command = new SqlCommand("SELECT * FROM UserAccounts WHERE username = @username", connection);
            command.Parameters.AddWithValue("@username", username);

            var reader = await command.ExecuteReaderAsync();

            reader.Read();
            return (int)reader["Attempts"];
        }

        return -1;
    }

    public void ResetFailedAttempts(string username)
    {
        var userAuthenticator = new User();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var command = new SqlCommand("UPDATE UserAccounts SET Attempts = 0", connection);
            command.ExecuteNonQuery();

        }
    }
}
