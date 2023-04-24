using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;


namespace AA.PMTOGO.DAL
{
    public class LoggerDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.LogDB;Trusted_Connection=True";

        public async Task<Result> InsertLog(Log log)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into Logs VALUES(@logId, @operation, @logLevel, @logCategory, @message, @timestamp)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@logId", log.LogId);
                command.Parameters.AddWithValue("@operation", log.Operation);
                command.Parameters.AddWithValue("@logLevel", log.LogLevel);
                command.Parameters.AddWithValue("@logCategory", log.Category);
                command.Parameters.AddWithValue("@message", log.Message);
                command.Parameters.AddWithValue("@timestamp", log.Timestamp);

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

        public async Task<Result> GetAnalysisLogs(string operation)
        {
            var result = new Result();

            var currentDate = DateTime.Now;
            var minDate = DateTime.Now.AddMonths(-3);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT COUNT(*) FROM Logs WHERE Operation = @operation AND (Timestamp BETWEEN @minDate AND @currentDate) GROUP BY  DAY(Timestamp)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@operation", operation);
                command.Parameters.AddWithValue("@minDate", minDate);
                command.Parameters.AddWithValue("@currentDate", currentDate);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        //create analysis from query 
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
    }
}
