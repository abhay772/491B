using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;

namespace AA.PMTOGO.DAL
{
    //logging
    public class ServiceDAO : IServiceDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.ServiceDB;Trusted_Connection=True";
        // Service Provider - Services DAO

        public ServiceDAO()
        {

        }
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
                        List<Object> listOfservice = new List<Object>();
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

        //single service
        public async Task<Result> FindService(string serviceName, string serviceProviderEmail, string serviceType)
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM UserServices WHERE @ServiceProviderEmail = serviceProviderEmail AND @ServiceName = serviceName";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviderEmail);
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

        public async Task<List<Service>> FindServicesWithQuery(string userQuery, int PageNumber, int PageLimit)
        {
            int OFFSET = (PageNumber - 1) * PageLimit;


            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT Id, Name AS ServiceName, Type AS ServiceType, ServiceDescription, Price AS ServicePrice " +
                                  "FROM Services " +
                                  "WHERE CONTAINS(ServiceDescription, @Query) " +
                                  "OFFSET @Offset ROWS " +
                                  "FETCH NEXT @PageSize ROWS ONLY";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Query", userQuery);
                command.Parameters.AddWithValue("@Offset", OFFSET);
                command.Parameters.AddWithValue("@PageSize", PageLimit);


                List<Service> services = new List<Service>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string name = (string)reader["ServiceName"];
                        string type = (string)reader["ServiceType"];
                        string description = (string)reader["ServiceDescription"];
                        decimal price = (decimal)reader["ServicePrice"];

                        Service service = new Service(id, name, type, description, price);
                        services.Add(service);
                    }
                }
                return services;
            }

        }

        //insert a service

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

        //delete a service

        public async Task<Result> DeleteService(string serviceName, string serviceType, string serviceProviderEmail)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DELETE FROM Services WHERE ServiceName = @serviceName AND ServiceProviderEmail = @serviceProviderEmail", connection);

                command.Parameters.AddWithValue("@serviceName", serviceName);
                command.Parameters.AddWithValue("@serviceProviderEmail", serviceProviderEmail);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (serviceType.Equals(reader["ServiceType"]))
                        {
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
                    }
                }
            }

            result.IsSuccessful = false;
            return result;
        }
    }
}
