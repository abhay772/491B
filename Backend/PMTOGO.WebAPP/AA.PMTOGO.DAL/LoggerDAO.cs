﻿using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;
using System.Text;

namespace AA.PMTOGO.DAL
{
    public class LoggerDAO : ILoggerDAO
    {


        private readonly string _connectionString;

        public LoggerDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("UsersDbConnectionString")!;
        }

        //private string _connectionString = "Server=.\\SQLEXPRESS;Database=AA.LogDB;Trusted_Connection=True;Encrypt=false";
        public async Task<Result> InsertLog(Log log)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT into Logs VALUES(@LogId, @Timestamp, @LogLevel, @Operation, @LogCategory, @Message)";

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
            Result result = new Result();
            try
            {
                IDictionary<DateTime, int> data = new Dictionary<DateTime, int>();

                // SQL query to select counts and select the days from the last 3 months where a user login and group by day
                string query = "SELECT COUNT(*) AS operation_count, CONVERT(DATE, Timestamp) AS operation_day " +
                               "FROM Logs WHERE Operation = @Operation " +
                               "AND Timestamp >= DATEADD(month, -3, GETDATE()) " +
                               "GROUP BY CONVERT(DATE, Timestamp)";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@Operation", operation);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        // Get the login count and day for each record in the result set
                        int loginCount = (int)reader["operation_count"];
                        DateTime loginDay = (DateTime)reader["operation_day"];

                        Console.WriteLine($"Login count for {loginDay.ToShortDateString()}: {loginCount}");

                        data.Add(loginDay, loginCount);

                    }
                    reader.Close();
                    result.IsSuccessful = true;
                    result.Payload = data;
                    return result;

                }
            }
            catch
            {
                Console.ReadLine();
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid Username or Passphrase. Please try again later.";
                return result;
            }

        }
    }
}