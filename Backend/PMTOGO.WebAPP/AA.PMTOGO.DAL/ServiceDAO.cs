
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;


namespace AA.PMTOGO.DAL
{
    //logging
    public class ServiceDAO: IServiceDAO
    {
        private readonly string _connectionString;
        //logging

        public ServiceDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ServiceDbConnectionString")!;
        }

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

                string sqlQuery = "SELECT Id, ServiceProvider, ServiceProviderEmail, ServiceName, ServiceType, ServiceDescription, ServicePrice FROM Services";

                var command = new SqlCommand(sqlQuery, connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        List<Service> listOfservice = new List<Service>();
                        while (reader.Read())
                        {
                            Service service = new Service((Guid)reader["Id"], (string)reader["ServiceName"], (string)reader["ServiceType"], (string)reader["ServiceDescription"], (string)reader["ServiceProvider"],
                                (string)reader["ServiceProviderEmail"], (double)reader["ServicePrice"]);

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
        public async Task<Result> FindService(Guid id)
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT Id, ServiceProvider, ServiceProviderEmail, ServiceName, ServiceType, ServiceDescription, ServicePrice FROM Services WHERE Id = @ID";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ID", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (id.Equals(reader["Id"]))
                        {
                            Service service = new Service((Guid)reader["Id"], (string)reader["ServiceName"], (string)reader["ServiceType"], (string)reader["ServiceDescription"], (string)reader["ServiceProvider"],
                                (string)reader["ServiceProviderEmail"], (double)reader["ServicePrice"]);

                            result.IsSuccessful = true;
                            result.ErrorMessage = "Service already exists.";
                            result.Payload = service;
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

        public async Task<Result> AddService(Service service)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into Services VALUES(@Id, @ServiceProvider,@ServiceProviderEmail, @ServiceName, @ServiceType, @ServiceDescription, @ServicePrice)";
                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Id", service.Id);
                command.Parameters.AddWithValue("@ServiceProvider", service.ServiceProvider);
                command.Parameters.AddWithValue("@ServiceProviderEmail", service.ServiceProviderEmail);
                command.Parameters.AddWithValue("@ServiceName", service.ServiceName);
                command.Parameters.AddWithValue("@ServiceType", service.ServiceType);
                command.Parameters.AddWithValue("@ServiceDescription", service.ServiceDescription);
                command.Parameters.AddWithValue("@ServicePrice", service.ServicePrice);


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

        public async Task<Result> DeleteService(Guid id)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DELETE FROM Services WHERE Id = @ID", connection);

                command.Parameters.AddWithValue("@ID", id);

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
