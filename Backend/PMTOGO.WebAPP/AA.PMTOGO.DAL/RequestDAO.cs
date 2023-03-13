﻿using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;


namespace AA.PMTOGO.DAL
{
    public class RequestDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.ServiceDB;Trusted_Connection=True;Encrypt=false";
        private readonly ILogger? _logger;

        public async Task<Result> GetUserRequest(string username)
        {
            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM ServiceRequests WHERE @Username = username";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    reader.Read();
                    try
                    {
                        List<ServiceRequest> listOfrequest = new List<ServiceRequest>();
                        while (reader.Read())
                        {

                            ServiceRequest request = new ServiceRequest();

                            request.RequestId = (Guid)reader["RequestId"];
                            request.ServiceName = (string)reader["ServiceName"];
                            request.ServiceType = (string)reader["ServiceType"];
                            request.ServiceDescription = (string)reader["ServiceDescription"];
                            request.ServiceFrequency = (string)reader["ServiceFrequency"];
                            request.Comments = (string)reader["Comments"];
                            request.ServiceProviderEmail = (string)reader["ServiceProviderEmail"];
                            request.ServiceProviderName = (string)reader["ServiceProviderName"];
                            request.PropertyManagerName = (string)reader["PropertyManagerName"];
                            request.PropertyManagerEmail = (string)reader["PropertyManagerEmail"];


                            listOfrequest.Add(request);
                            result.IsSuccessful = true;
                            result.Payload = listOfrequest;

                        }
                        return result;
                    }
                    catch
                    {

                        result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                        result.IsSuccessful = false;
                       // _logger!.Log("FindUser", 4, LogCategory.Server, result);

                    }
                }
            }
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid Username or Passphrase. Please try again later.";
            return result;
        }
        public async Task<Result> RateUserService(Guid id, int rate)
        {
            Result result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("UPDATE UserServices SET Rating = rate WHERE @ServiceId = id", connection);
                command.Parameters.AddWithValue("@ServiceId", id);
                command.Parameters.AddWithValue("@Rating", rate);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    reader.Read();
                    try
                    {
                        reader.Read();
                        if ((int)reader["Rating"] == rate)
                        {
                            result.IsSuccessful = true;
                            return result;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                        }
                    }
                    catch
                    {
                        result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                        result.IsSuccessful = false;
                        _logger!.Log("RateUserService", 4, LogCategory.Server, result);
                    }

                }
            }
            return result;
        }

        public async Task<Result> AddService(Guid serviceId, string serviceName, string serviceType, string serviceDescription, string serviceFrequency, 
            string serviceProviderEmail, string serviceProviderName, string propertyManagerEmail, string propertyManagerName)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into UserServices VALUES(@ServiceId, @ServiceName, @ServiceType, @ServiceDescription, @ServiceFrequency, @ServiceProviderEmail, @ServiceProvider, @PropertyManagerEmail, @PropertyManagerName, @Status, @Rating)";

                
                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceId", serviceId);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@ServiceType", serviceType);
                command.Parameters.AddWithValue("@ServiceDescription", serviceDescription);
                command.Parameters.AddWithValue("@ServiceFrequency", serviceFrequency);
                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail);
                command.Parameters.AddWithValue("@ServiceProvider", serviceProviderName);
                command.Parameters.AddWithValue("@PropertyManagerEmail", propertyManagerEmail);
                command.Parameters.AddWithValue("@PropertyManagerName", propertyManagerName);
                command.Parameters.AddWithValue("@Status", "In-Progress");
                command.Parameters.AddWithValue("@Rating", null);

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
                        _logger!.Log("AcceptService", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        public async Task<Result> AddRequest(Guid requestId, string serviceName, string serviceType, string serviceDescription,
                 string serviceFrequency, string comments, string serviceProviderEmail, string serviceProviderName, string propertyManagerName, string propertyManagerEmail)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into ServiceRequests VALUES(@RequestId, @ServiceName, @ServiceType, @ServiceDescription, @ServiceFrequency, @Comments, @ServiceProviderEmail, @ServiceProviderName, @PropertyManagerEmail, @PropertyManagerName)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@RequestId", requestId);             
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@ServiceType", serviceType);
                command.Parameters.AddWithValue("@ServiceDescription", serviceDescription);     
                command.Parameters.AddWithValue("@ServiceFrequency", serviceFrequency);
                command.Parameters.AddWithValue("@Comments", comments);
                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail);
                command.Parameters.AddWithValue("@ServiceProviderName", serviceProviderName);
                command.Parameters.AddWithValue("@PropertyManagerEmail", propertyManagerEmail);
                command.Parameters.AddWithValue("@PropertyManagerName", propertyManagerName);


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
                        //_logger!.Log("AddRequest", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        public async Task<Result> DeleteServiceRequest(Guid serviceId)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "DELETE FROM ServiceRequests WHERE @ServiceId = serviceId)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceId", serviceId);

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
                        _logger!.Log("DeleteServiceRequest", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

    }

}
