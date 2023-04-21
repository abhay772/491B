﻿using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;


namespace AA.PMTOGO.DAL;

public class UsersDAO
{
    //logging
    private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.UsersDB;Trusted_Connection=True";

    //for account authentication // look for the users username/unique ID in sensitive info Table UserAccount
    public async Task<Result> FindUser(string username)
    {
        Result result = new Result();

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

    public async Task<Result> ActivateUser(string username)
    {
        var result = new Result();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "UPDATE UserAccounts SET IsActive = 1 WHERE Username = @Username";

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
}
