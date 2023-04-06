using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace AA.PMTOGO.DAL
{
    public class ServiceRequestDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.ServiceDB;Trusted_Connection=True";

        //Find request or userservice

        public async Task<Result> FindServiceRequest(Guid id) //single request
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM ServiceRequests WHERE ID = @Id";

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

       
        public async Task<Result> GetAServiceRequest(Guid requestId) // return single request
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM ServiceRequests WHERE @Id = requestId";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Id", requestId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (requestId.Equals(reader["Id"]))
                        {
                            ServiceRequest request = new ServiceRequest((Guid)reader["Id"], (string)reader["ServiceName"], (string)reader["ServiceType"], (string)reader["ServiceDescription"],
                                (string)reader["ServiceFrequency"], (string)reader["Comments"], (string)reader["ServiceProviderEmail"], (string)reader["ServiceProviderName"],
                               (string)reader["PropertyManagerEmail"], (string)reader["PropertyManagerName"]);


                            result.IsSuccessful = true;
                            result.Payload = request;
                            return result;
                        }
                    }
                }
                result.IsSuccessful = false;
                return result;
            }

        }

        public async Task<Result> GetServiceRequests(string serviceProviderEmail) // list of request
        {

            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM ServiceRequests WHERE @ServiceProviderEmail = serviceProviderEmail";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        List<ServiceRequest> listOfrequest = new List<ServiceRequest>();
                        while (reader.Read())
                        {

                            ServiceRequest request = new ServiceRequest((Guid)reader["Id"], (string)reader["ServiceName"], (string)reader["ServiceType"], (string)reader["ServiceDescription"],
                                (string)reader["ServiceFrequency"], (string)reader["Comments"], (string)reader["ServiceProviderEmail"], (string)reader["ServiceProviderName"],
                               (string)reader["PropertyManagerEmail"], (string)reader["PropertyManagerName"]);


                            listOfrequest.Add(request);

                        }
                        result.IsSuccessful = true;
                        result.Payload = listOfrequest;
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

        //insert user service

        public async Task<Result> AddUserService(Guid Id, string serviceName, string serviceType, string serviceDescription,
string serviceFrequency, string serviceProviderEmail, string serviceProviderName, string propertyManagerEmail, string propertyManagerName)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into UserServices VALUES(@Id, @ServiceName, @ServiceType, @ServiceDescription, @ServiceFrequency, @ServiceProviderEmail, @ServiceProviderName, @PropertyManagerEmail, @PropertyManagerName, @Status, @Rating)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@ServiceType", serviceType);
                command.Parameters.AddWithValue("@ServiceDescription", serviceDescription);
                command.Parameters.AddWithValue("@ServiceFrequency", serviceFrequency);
                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail);
                command.Parameters.AddWithValue("@ServiceProviderName", serviceProviderName);
                command.Parameters.AddWithValue("@PropertyManagerEmail", propertyManagerEmail);
                command.Parameters.AddWithValue("@PropertyManagerName", propertyManagerName);
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


        public async Task<Result> DeleteServiceRequest(Guid requestId)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DELETE FROM ServiceRequests WHERE ID = @Id", connection);

                command.Parameters.AddWithValue("@ID", requestId);

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

    }

}
/* pass in the query, and dependents 1)insert 2)select 3)delete*/