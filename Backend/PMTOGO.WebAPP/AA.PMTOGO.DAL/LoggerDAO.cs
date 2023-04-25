using AA.PMTOGO.Models.Entities;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace AA.PMTOGO.DAL
{
    public class LoggerDAO : ILoggerDAO
    {
        private readonly string _connectionString;
        //private readonly ILogger? _logger;
        //logging

        public LoggerDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("UsersDbConnectionString")!;
        }

        public async Task<Result> InsertLog(Log log)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into Logs VALUES(@LogId, @Timestamp, @LogLevel, @Operation, @LogCategory, @Message)";
                //string sqlQuery = "INSERT into Logs VALUES(@LogId, @Operation, @LogLevel, @LogCategory, @Message, @Timestamp)";

                var command = new SqlCommand(sqlQuery, connection);
                
                command.Parameters.AddWithValue("@LogId", log.LogId);
                command.Parameters.AddWithValue("@Operation", log.Operation);
                command.Parameters.AddWithValue("@LogLevel", log.LogLevel);
                command.Parameters.AddWithValue("@LogCategory", log.Category);
                command.Parameters.AddWithValue("@Message", log.Message);
                command.Parameters.AddWithValue("@Timestamp", log.Timestamp);

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

        public async Task<Result> GetAnalysisLogs(string operation)
        {
            var result = new Result();

            var currentDate = DateTime.Now;
            var minDate = DateTime.Now.AddMonths(-3);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //string sqlQuery = "SELECT COUNT(*), Timestamp FROM Logs WHERE Operation = @Operation AND Timestamp BETWEEN @minDate AND @currentDate GROUP BY DAY(Timestamp)";
                string query = "SELECT COUNT(*) FROM (SELECT Timestamp, convert(date, Timestamp) as the_date, COUNT(*) over (partition by Timestamp) as num_dates FROM Logs WHERE Timestamp >= @minDate AND Timestamp < dateadd(day, 1, @currentDate) AND Operation = @Operation Group By Timestamp, convert(date, Timestamp))Logs WHERE num_dates = datediff(day, @currentDate, @minDate) + 1;";
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Operation", operation);
                command.Parameters.AddWithValue("@minDate", minDate);
                command.Parameters.AddWithValue("@currentDate", currentDate);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        //create analysis from query 

                        result.IsSuccessful = true;
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
