using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.DAL
{
    //logging
    public class UserServiceDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.ServiceDB;Trusted_Connection=True";

        //find user service

        public async Task<Result> FindUserService(Guid id) //single request
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //change select star
                string sqlQuery = "SELECT * FROM UserServices WHERE ID = @Id";

                var command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;


                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (id.Equals(reader["ID"]))
                        {
                            result.IsSuccessful = true;
                            result.ErrorMessage = "Service Request already exists.";
                            return result;
                        }
                    }
                }
                result.IsSuccessful = false;
                return result;
            }

        }

        //insert user service

        public async Task<Result> AddUserService(ServiceRequest service)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into UserServices VALUES(@Id, @ServiceName, @ServiceType, @ServiceDescription, @ServiceFrequency, @ServiceProviderEmail, @ServiceProviderName, @PropertyManagerEmail, @PropertyManagerName, @Status, @Rating)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Id", service.Id);
                command.Parameters.AddWithValue("@ServiceName", service.ServiceName);
                command.Parameters.AddWithValue("@ServiceType", service.ServiceType);
                command.Parameters.AddWithValue("@ServiceDescription", service.ServiceDescription);
                command.Parameters.AddWithValue("@ServiceFrequency", service.ServiceFrequency);
                command.Parameters.AddWithValue("@ServiceProviderEmail", service.ServiceProviderEmail);
                command.Parameters.AddWithValue("@ServiceProviderName", service.ServiceProviderName);
                command.Parameters.AddWithValue("@PropertyManagerEmail", service.PropertyManagerEmail);
                command.Parameters.AddWithValue("@PropertyManagerName", service.PropertyManagerName);
                command.Parameters.AddWithValue("@Status", "In-Progress");
                command.Parameters.AddWithValue("@Rating", 0);


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

        

        //user service list
        public async Task<Result> GetUserService(string sqlQuery, string email, string rating) // return all user services
        {

            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //change select star ~ is it a service provider or property manager
                //to return user services for both service provider and property manager

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@PropertyManagerEmail", email);
                command.Parameters.AddWithValue("@ServiceProviderEmail", email);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        List<UserService> listOfUserServices = new List<UserService>();
                        while (reader.Read())
                        {

                            UserService service = new UserService((Guid)reader["Id"], (string)reader["ServiceName"], (string)reader["ServiceType"], (string)reader["ServiceDescription"],
                                (string)reader["ServiceFrequency"], (string)reader["ServiceProviderEmail"], (string)reader["ServiceProviderName"],
                                (string)reader["PropertyManagerEmail"], (string)reader["PropertyManagerName"], (string)reader["Status"], (int)reader[rating]);


                            listOfUserServices.Add(service);

                        }
                        result.IsSuccessful = true;
                        result.Payload = listOfUserServices;
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


        //frequency change if accepted by service provider

        public async Task<Result> UpdateServiceFrequency(Guid id, string frequency)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserServices SET serviceFrequncy = @ServiceFrequency WHERE Id = @ID";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ID", SqlDbType.UniqueIdentifier).Value = id;
                command.Parameters.AddWithValue("@ServiceFrequency", SqlDbType.Int).Value = frequency;

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

                catch 
                {
                    result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                    result.IsSuccessful = false;
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        //cancellation ~ delete user service if confirmed by service provider

        public async Task<Result> DeleteUserService(Guid requestId)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DELETE FROM UserServices WHERE ID = @Id", connection);

                command.Parameters.AddWithValue("@Id", requestId);

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

        //Rating
        public async Task<Result> UpdateServiceRate(Guid Id, int rating, string query)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ID", SqlDbType.UniqueIdentifier).Value = Id;
                command.Parameters.AddWithValue("@Rating", SqlDbType.Int).Value= rating;

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

                catch 
                {
                    result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                    result.IsSuccessful = false;
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        //check rating and frequency functions
        public async Task<Result> CheckRating(Guid Id, int rating, string sqlQuery, string userRate)
        {
            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ID", SqlDbType.UniqueIdentifier).Value = Id;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        reader.Read();
                        if ((int)reader[userRate] == rating)
                        {
                            result.IsSuccessful = true;
                            return result;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.ErrorMessage = "Rate is incorrect";
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
            result.ErrorMessage = "Invalid Service ID or Property Manager Email. Please try again later.";
            return result;
        }

        public async Task<Result> CheckFrequency(Guid id, string frequency )
        {
            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT ServiceFrequency FROM UserServices WHERE Id = @ID";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ID", SqlDbType.UniqueIdentifier).Value = id;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        reader.Read();
                        if ((string)reader["ServiceFreqency"] == frequency)
                        {
                            result.IsSuccessful = true;
                            return result;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.ErrorMessage = "Rate is incorrect";
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
            result.ErrorMessage = "Invalid Service ID or Property Manager Email. Please try again later.";
            return result;
        }


    }
}
