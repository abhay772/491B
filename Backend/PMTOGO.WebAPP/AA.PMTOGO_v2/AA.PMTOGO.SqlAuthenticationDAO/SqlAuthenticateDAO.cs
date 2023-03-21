using AA.PMTOGO.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace AA.PMTOGO.SqlAuthenticationDAO;

public class SqlAuthenticateDAO
{
    private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.Users;Trusted_Connection=True;Encrypt=false";

    public Result Authenticate(string username, string password)
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
                reader.Read();

                if ((string)reader["attempts"] == "3")
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Account is Locked";
                    return result;
                }
                if (reader["username"].Equals(username))
                {

                    if (password.Equals(reader["Password"]))
                    {
                        command = new SqlCommand("UPDATE UserAccounts SET Attempts = 0", connection);

                        command.ExecuteNonQuery();
                        result.IsSuccessful = true;
                        return result;
                    }
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Incorrect Username or Password";
                    return result;
                }
                result.IsSuccessful = false;
                result.ErrorMessage = "Incorrect Username or Password";
                return result;

            }
        }
        /*result.IsSuccessful = true;
        return result;*/
    }
    public byte[] EncrpytPassword(string password, byte[] salt)
    {

        var pass = Encoding.UTF8.GetBytes(password);
        Console.WriteLine(pass);

        // Lecture Vong 12/13 
        var hash = new Rfc2898DeriveBytes(pass, salt, 1000, HashAlgorithmName.SHA512);
        var encryptedPass = hash.GetBytes(64);

        return encryptedPass;
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
        
    }
}
