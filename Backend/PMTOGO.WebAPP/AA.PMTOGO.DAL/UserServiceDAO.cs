using AA.PMTOGO.Models.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Text;


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

        //insert service request

        public async Task<Result> AddServiceRequest(Guid Id, string serviceName, string serviceType, string serviceDescription,
         string serviceFrequency, string comments, string serviceProviderEmail, string serviceProviderName, string propertyManagerEmail, string propertyManagerName)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into ServiceRequests VALUES(@Id, @serviceName, @serviceType, @serviceDescription, @serviceFrequency, @comments, @serviceProviderEmail, @serviceProviderName, @propertyManagerEmail, @propertyManagerName)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Id", Id);
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

        //user service list
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

                            UserService service = new UserService((Guid)reader["Id"], (string)reader["ServiceName"], (string)reader["ServiceType"], (string)reader["ServiceDescription"],
                                (string)reader["ServiceFrequency"], (string)reader["ServiceProviderEmail"], (string)reader["ServiceProviderName"],
                                (string)reader["PropertyManagerEmail"], (string)reader["PropertyManagerName"], (string)reader["Status"], (int)reader["Rating"]);


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


        //frequency change

        //cancellation ~ delete user service

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
        public async Task<Result> RateUserServices(Guid serviceId, int rating)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserServices SET rating = @Rating WHERE Id = @ID";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ID", SqlDbType.UniqueIdentifier).Value = serviceId;
                command.Parameters.AddWithValue("@Rating", SqlDbType.Int).Value = rating;

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

                string sqlQuery = "SELECT Rating FROM UserServices WHERE @serviceId = ID";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@serviceId", serviceId);

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
