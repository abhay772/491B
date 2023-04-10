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
    }
}
