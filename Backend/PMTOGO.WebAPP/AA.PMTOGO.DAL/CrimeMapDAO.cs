using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;

namespace AA.PMTOGO.DAL
{
    public class CrimeMapDAO : ICrimeMapDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.CrimeMapDB;Trusted_Connection=True";
        private CrimeAlert _crimeAlert;
        private Result _result;
        public CrimeMapDAO(CrimeAlert crimeAlert, Result result)
        {
            _crimeAlert = crimeAlert;
            _result = result;
        }

        public async Task<Result> AddAlert(CrimeAlert alert)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT INTO CrimeAlerts (Email, Name, Location, Description, Time, Date, X, Y) VALUES (@Email, @Name, @Location, @Description, @Time, @Date, @X, @Y); SELECT SCOPE_IDENTITY();";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Email", alert.Email);
                command.Parameters.AddWithValue("@Name", alert.Name);
                command.Parameters.AddWithValue("@Location", alert.Location);
                command.Parameters.AddWithValue("@Description", alert.Description);
                command.Parameters.AddWithValue("@Time", alert.Time);
                command.Parameters.AddWithValue("@Date", alert.Date);
                command.Parameters.AddWithValue("@X", alert.X);
                command.Parameters.AddWithValue("@Y", alert.Y);

                try
                {
                    int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());

                    _result.IsSuccessful = true;
                    return _result;
                }
                catch (SqlException e)
                {
                    _result.IsSuccessful = false;
                    _result.ErrorMessage = e.Message;
                    return _result;
                }
            }
        }

        public async Task<Result> CheckAlert(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT COUNT(*) FROM CrimeAlerts WHERE Email = @Email";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    int rowCount = (int)await command.ExecuteScalarAsync();

                    _result.IsSuccessful = rowCount <= 1;

                    return _result;
                }
                catch (SqlException e)
                {
                    _result.IsSuccessful = false;
                    _result.ErrorMessage = e.Message;

                    return _result;
                }
            }
        }

        public async Task<Result> DeleteAlert(string email, string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "DELETE FROM CrimeAlerts WHERE Email = @Email AND ID = @ID";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@ID", id);

                try
                {
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    _result.IsSuccessful = rowsAffected == 1;

                    return _result;
                }
                catch (SqlException e)
                {
                    _result.IsSuccessful = false;
                    _result.ErrorMessage = e.Message;

                    return _result;
                }
            }
        }

        public async Task<Result> EditAlert(string email, string id, CrimeAlert alert)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE CrimeAlerts SET Name = @Name, Location = @Location, Description = @Description, Time = @Time, Date = @Date, X = @X, Y = @Y WHERE Email = @Email AND ID = @ID";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Name", alert.Name);
                command.Parameters.AddWithValue("@Location", alert.Location);
                command.Parameters.AddWithValue("@Description", alert.Description);
                command.Parameters.AddWithValue("@Time", alert.Time);
                command.Parameters.AddWithValue("@Date", alert.Date);
                command.Parameters.AddWithValue("@X", alert.X);
                command.Parameters.AddWithValue("@Y", alert.Y);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@ID", id);

                try
                {
                    var rows = await command.ExecuteNonQueryAsync();

                    if (rows == 1)
                    {
                        _result.IsSuccessful = true;
                        return _result;
                    }
                    else
                    {
                        _result.IsSuccessful = false;
                        _result.ErrorMessage = "No rows affected";
                        return _result;
                    }
                }
                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        _result.IsSuccessful = false;
                        _result.ErrorMessage = "Specified table not found";
                    }
                    else
                    {
                        _result.IsSuccessful = false;
                        _result.ErrorMessage = e.Message;
                    }
                }
            }

            _result.IsSuccessful = false;
            _result.ErrorMessage = "Error";
            return _result;
        }

        public async Task<List<CrimeAlert>> GetAlerts()
        {
            var alerts = new List<CrimeAlert>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM CrimeAlerts";
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                var command = new SqlCommand(sqlQuery, connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        while (await reader.ReadAsync())
                        {
                            var alert = new CrimeAlert();
                            alert.Email = (string)reader["Email"];
                            //alert.ID = (string)reader["ID"];
                            alert.Name = (string)reader["Name"];
                            alert.Location = (string)reader["Location"];
                            alert.Description = (string)reader["Description"];
                            alert.Time = (string)reader["Time"];
                            alert.Date = (string)reader["Date"];
                            alert.X = (double)reader["X"];
                            alert.Y = (double)reader["Y"];

                            alerts.Add(alert);
                            Console.WriteLine(alert.Date);
                            Console.WriteLine(alert.Description);
                        }
                    }
                    catch { }
                }
            }

            return alerts;
        }

        public async Task<CrimeAlert> ViewAlert(string email, string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM CrimeAlerts WHERE Email = @Email AND ID = @ID";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@ID", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    try 
                    { 
                        if (await reader.ReadAsync())
                        {
                            var alert = new CrimeAlert();
                            alert.Email = (string)reader["Email"];
                            alert.ID = (string)reader["ID"];
                            alert.Name = (string)reader["Name"];
                            alert.Location = (string)reader["Location"];
                            alert.Description = (string)reader["Description"];
                            alert.Time = (string)reader["Time"];
                            alert.Date = (string)reader["Date"];
                            alert.X = (double)reader["X"];
                            alert.Y = (double)reader["Y"];

                            return alert;
                        }
                    }
                    catch { }
                    
                }
            }

            return null;
        }
    }
}
