using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace AA.PMTOGO.DAL
{
    public class RequestDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.ServiceDB;Trusted_Connection=True;Encrypt=false";



        //Service Request
        public async Task<Result> FindRequest(Guid requestId) //single request
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM ServiceRequests WHERE @RequestId = requestId";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@RequestId", requestId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (requestId.Equals(reader["RequestId"]))
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
        public async Task<Result> GetARequest(Guid requestId) // return single request
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM ServiceRequests WHERE @RequestId = requestId";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@RequestId", requestId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (requestId.Equals(reader["RequestId"]))
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
                            request.PropertyManagerEmail = (string)reader["PropertyManagerEmail"];
                            request.PropertyManagerName = (string)reader["PropertyManagerName"];


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

        public async Task<Result> GetServiceRequest(string serviceProviderEmail) // list of request
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

                            ServiceRequest request = new ServiceRequest();

                            request.RequestId = (Guid)reader["RequestId"];
                            request.ServiceName = (string)reader["ServiceName"];
                            request.ServiceType = (string)reader["ServiceType"];
                            request.ServiceDescription = (string)reader["ServiceDescription"];
                            request.ServiceFrequency = (string)reader["ServiceFrequency"];
                            request.Comments = (string)reader["Comments"];
                            request.ServiceProviderEmail = (string)reader["ServiceProviderEmail"];
                            request.ServiceProviderName = (string)reader["ServiceProviderName"];
                            request.PropertyManagerEmail = (string)reader["PropertyManagerEmail"];
                            request.PropertyManagerName = (string)reader["PropertyManagerName"];


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

        public async Task<Result> AddRequest(Guid requestId, string serviceName, string serviceType, string serviceDescription,
                 string serviceFrequency, string comments, string serviceProviderEmail, string serviceProviderName, string propertyManagerEmail, string propertyManagerName)
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

                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        //User Services DAO

        public async Task<Result> FindUserService(Guid serviceid)     // single user service   
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM UserServices WHERE @ServiceId = serviceid";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceId", serviceid);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (serviceid.Equals(reader["ServiceId"]))
                        {

                            result.IsSuccessful = true;
                            result.ErrorMessage = "UserService already exists.";
                            return result;

                        }
                    }
                }

                
                result.IsSuccessful = false;
                return result;
            }

        }
        

        public async Task<Result> GetUserService(string serviceProviderEmail, string propertyManagerEmail) // return all user services
        {

            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM UserServices WHERE @PropertyManagerEmail = propertyManagerEmail OR @ServiceProviderEmail = serviceProviderEmail";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@PropertyManagerEmail", propertyManagerEmail);
                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        List<UserService> listOfUserServices = new List<UserService>();
                        while (reader.Read())
                        {

                            UserService service = new UserService();

                            service.ServiceId = (Guid)reader["ServiceId"];
                            service.ServiceName = (string)reader["ServiceName"];
                            service.ServiceType = (string)reader["ServiceType"];
                            service.ServiceDescription = (string)reader["ServiceDescription"];
                            service.ServiceFrequency = (string)reader["ServiceFrequency"];
                            service.ServiceProviderEmail = (string)reader["ServiceProviderEmail"];
                            service.ServiceProvider = (string)reader["ServiceProvider"];
                            service.PropertyManagerEmail = (string)reader["PropertyManagerEmail"];
                            service.PropertyManagerName = (string)reader["PropertyManagerName"];
                            service.Status = (string)reader["Status"];
                            service.Rating = (int)reader["Rating"];


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

        public async Task<Result> AddUserService(Guid serviceId, string serviceName, string serviceType, string serviceDescription,
                string serviceFrequency, string serviceProviderEmail, string serviceProvider, string propertyManagerEmail, string propertyManagerName)
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
                command.Parameters.AddWithValue("@ServiceProvider", serviceProvider);
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

        // Service Provider - Services DAO
        public async Task<Result> GetServices() //list of services
        {

            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM Services";

                var command = new SqlCommand(sqlQuery, connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        List<Service> listOfservice = new List<Service>();
                        while (reader.Read())
                        {

                            Service service = new Service();

                            service.ServiceProvider = (string)reader["ServiceProvider"];
                            service.ServiceProviderEmail = (string)reader["ServiceProviderEmail"];
                            service.ServiceName = (string)reader["ServiceName"];
                            service.ServiceType = (string)reader["ServiceType"];
                            service.ServiceDescription = (string)reader["ServiceDescription"];



                            listOfservice.Add(service);

                        }
                        result.IsSuccessful = true;
                        result.Payload = listOfservice;
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


        public async Task<Result> FindService(string serviceName, string serviceProviderEmail, string serviceType)//single service
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM UserServices WHERE @ServiceProviderEmail = serviceProviderEmail AND @ServiceName = serviceName";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail );
                command.Parameters.AddWithValue("@ServiceName", serviceName);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (serviceProviderEmail.Equals(reader["ServiceProviderEmail"]) && serviceName.Equals(reader["ServiceName"]) && serviceType.Equals(reader["ServiceType"]))
                        {

                            result.IsSuccessful = true;
                            result.ErrorMessage = "Service already exists.";
                            return result;

                        }
                    }
                }


                result.IsSuccessful = false;
                return result;
            }

        }

        public async Task<Result> AddService(string serviceName, string serviceType, string serviceDescription,
                string serviceProviderEmail, string serviceProvider)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into Services VALUES(@ServiceProvider,@ServiceProviderEmail, @ServiceName, @ServiceType, @ServiceDescription)";
                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceProvider", serviceProvider);
                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@ServiceType", serviceType);
                command.Parameters.AddWithValue("@ServiceDescription", serviceDescription);


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
        public async Task<Result> RateUserServices(Guid serviceId, int rating)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserServices SET @Rating = rating WHERE @ServiceId = serviceId";

                var command = new SqlCommand(sqlQuery, connection);
         
                command.Parameters.AddWithValue("@ServiceId", serviceId);
                command.Parameters.AddWithValue("@Rating", rating);

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

                catch (SqlException ex)
                {
                    StringBuilder errorMessages = new StringBuilder();
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append("Index #" + i + "\n" +
                            "Message: " + ex.Errors[i].Message + "\n" +
                            "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                            "Source: " + ex.Errors[i].Source + "\n" +
                            "Procedure: " + ex.Errors[i].Procedure + "\n");
                    }
                    Console.WriteLine(errorMessages.ToString());
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        public async Task<Result> CheckRating(Guid serviceId, int rating)
        {
            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT Rating FROM UserServices WHERE @ServiceId = serviceId";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceId", serviceId);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        reader.Read();
                        if ((int)reader["Rating"] == rating)
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
