using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.DAL
{
    //logging
    public class ServiceDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.ServiceDB;Trusted_Connection=True";
        // Service Provider - Services DAO
        public async Task<Result> GetServices() //list of services
        {

            Result result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT ServiceProvider, ServiceProviderEmail, ServiceName, ServiceType, ServiceDescription, ServicePrice FROM Services";

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
                            service.ServicePrice = (double)reader["ServicePrice"];



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

                string sqlQuery = "SELECT ServiceName, ServiceType, ServiceProviderEmail FROM UserServices WHERE @ServiceProviderEmail = serviceProviderEmail AND @ServiceName = serviceName";

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

        //insert a service

        public async Task<Result> AddService(Service service)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into Services VALUES(@Id, @ServiceProvider,@ServiceProviderEmail, @ServiceName, @ServiceType, @ServiceDescription, ServicePrice)";
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
