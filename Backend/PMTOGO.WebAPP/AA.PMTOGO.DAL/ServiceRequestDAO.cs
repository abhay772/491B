using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;

using System.Data.SqlClient;

namespace AA.PMTOGO.DAL
{
    //logging
    public class ServiceRequestDAO: IServiceRequestDAO
    {
         /*private readonly string _connectionString;
         //logging

         public ServiceRequestDAO(IConfiguration configuration)
         {
             _connectionString = configuration.GetConnectionString("ServiceDbConnectionString")!;
         }*/

        private string _connectionString = "Server=.\\SQLEXPRESS;Database=AA.ServiceDB;Trusted_Connection=True;Encrypt=false";

        //Find request or userservice

        public async Task<Result> FindServiceRequest(Guid id) //single request
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //change select star
                string sqlQuery = "SELECT Id FROM ServiceRequests WHERE ID = @Id";

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

                //change select star

                string sqlQuery = "SELECT Id, RequestType, ServiceName, ServiceType, ServiceDescription, ServiceFrequency, Comments, ServiceProviderEmail, ServiceProviderName, PropertyManagerEmail, PropertyManagerName FROM ServiceRequests WHERE ID = @Id";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Id", requestId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (requestId.Equals(reader["Id"]))
                        {
                            ServiceRequest request = new ServiceRequest((Guid)reader["Id"], (string)reader["RequestType"], (string)reader["ServiceName"], (string)reader["ServiceType"], (string)reader["ServiceDescription"],
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
                //change select star

                string sqlQuery = "SELECT Id, RequestType, ServiceName, ServiceType, ServiceDescription, ServiceFrequency, Comments, ServiceProviderEmail, ServiceProviderName, PropertyManagerEmail, PropertyManagerName FROM ServiceRequests WHERE @ServiceProviderEmail = serviceProviderEmail";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        List<ServiceRequest> listOfrequest = new List<ServiceRequest>();
                        while (reader.Read())
                        {

                            ServiceRequest request = new ServiceRequest((Guid)reader["Id"], (string)reader["RequestType"], (string)reader["ServiceName"], (string)reader["ServiceType"], (string)reader["ServiceDescription"],
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
        //insert service request

        public async Task<Result> AddServiceRequest(ServiceRequest request)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into ServiceRequests VALUES(@Id, @RequestType, @ServiceName, @ServiceType, @ServiceDescription, @ServiceFrequency, @Comments, @ServiceProviderEmail, @ServiceProviderName, @PropertyManagerEmail, @PropertyManagerName)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Id", request.Id);
                command.Parameters.AddWithValue("@RequestType", request.RequestType);
                command.Parameters.AddWithValue("@ServiceName", request.ServiceName);
                command.Parameters.AddWithValue("@ServiceType", request.ServiceType);
                command.Parameters.AddWithValue("@ServiceDescription", request.ServiceDescription);
                command.Parameters.AddWithValue("@ServiceFrequency", request.ServiceFrequency);
                command.Parameters.AddWithValue("@Comments", request.Comments);
                command.Parameters.AddWithValue("@ServiceProviderEmail", request.ServiceProviderEmail);
                command.Parameters.AddWithValue("@ServiceProviderName", request.ServiceProviderName);
                command.Parameters.AddWithValue("@PropertyManagerEmail", request.PropertyManagerEmail);
                command.Parameters.AddWithValue("@PropertyManagerName", request.PropertyManagerName);


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

    }

}
/* pass in the query, and dependents 1)insert 2)select 3)delete*/
