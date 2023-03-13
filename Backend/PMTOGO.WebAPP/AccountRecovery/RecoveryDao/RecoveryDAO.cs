using System.Data.SqlClient;

namespace AA.PMTOGO.SqlUserDAO
{
    public class RecoveryDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.UsersDB;Trusted_Connection=True;Encrypt=false";

        public RecoveryDAO(){}

        public Result DoesUserExist(string email)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM UserProfile WHERE Email = " + email;

                var command = new SqlCommand(sqlQuery, connection);

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
                string sqlQuery = "UPDATE UserAccount SET isActive = 0 WHERE username = " + username;
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
                string sqlQuery = "UPDATE UserAccount SET isActive = 1 WHERE username = " + username;
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
                    }
                }
            }
            result.IsSuccessful = false;
            return result;
        }

        public Result SetRecoveryRequest(string username)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sqlQuery = "UPDATE UserAccount SET RecoveryRequest = 1 WHERE username = " + username;
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
                    }
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
                using (SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE RecoveryRequest = 1", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.username = (string)reader["Email"];
                            user.RecoveryRequest = (bool)reader["RecoveryRequest"];
                            user.accountLocked = (bool)reader["AccountLocked"];
                            user.successfulOTP = (bool)reader["SuccessfulOTP"];
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        public Result SetSuccessfulOTP(string username)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserAccount SET successfulOTP = 1 WHERE username = " + username;

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
                    }
                }
            }
            result.IsSuccessful = false;
            return result;
        } 
    }
}