using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;


namespace AA.PMTOGO.DAL
{
    public class RequestDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.ServiceDB;Trusted_Connection=True;Encrypt=false";
        private readonly ILogger? _logger;

        public async Task<Result> GetUserRequest(string serviceProviderEmail)
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
                        //reader.Read();
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
                            
                            result.Payload = listOfrequest;

                        }
                        result.IsSuccessful = true;
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

        public async Task<Result> AddService(Guid serviceId, string serviceName, string serviceType, string serviceDescription,
                 string serviceFrequency, string serviceProviderEmail, string serviceProviderName, string propertyManagerName, string propertyManagerEmail)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into UserServices VALUES(@ServiceId, @ServiceName, @ServiceType, @ServiceDescription, @ServiceFrequency, @ServiceProviderEmail, @ServiceProviderName, @PropertyManagerEmail, @PropertyManagerName, @Status, @Rating)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceId", serviceId);
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
                        //_logger!.Log("AddService", 4, LogCategory.DataStore, result);
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

                var command = new SqlCommand("DELETE FROM ServiceRequests WHERE @RequestId = requestId", connection);

                command.Parameters.AddWithValue("@RequestId", requestId);

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
                        //_logger!.Log("DeleteServiceRequest", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        public async Task<Result> RateUserServices(Guid serviceId, int rate)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("UPDATE UserServices SET @Rating = rate WHERE @ServiceId = serviceId", connection);

                command.Parameters.AddWithValue("@ServiceId", serviceId);
                command.Parameters.AddWithValue("@Rating", rate);

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
                        //_logger!.Log("DeleteServiceRequest", 4, LogCategory.DataStore, result);
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

    }

}
