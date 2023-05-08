using AA.PMTOGO.Models.Entities;
using System.Data;
using System.Data.SqlClient;

namespace AA.PMTOGO.DAL
{
    public class DIYDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=DIYDB;Trusted_Connection=True";

        public DIYDAO() { }

        public bool TestConnection()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        public async Task<bool> UploadInfo(string email, string name, string description)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Check if there's already a row with the same email and name
                var checkCommand = new SqlCommand("SELECT COUNT(*) FROM DIYTable WHERE DIYEmail = @Email AND DIYName = @Name", connection);
                checkCommand.Parameters.AddWithValue("@Email", email);
                checkCommand.Parameters.AddWithValue("@Name", name);

                var rowCount = (int)await checkCommand.ExecuteScalarAsync();

                // If there's already a row with the same email and name, return false
                if (rowCount > 0)
                {
                    return false;
                }

                // If there's no row with the same email and name, insert the new row
                var insertCommand = new SqlCommand("INSERT INTO DIYTable (DIYEmail, DIYName, DIYDescription) VALUES (@Email, @Name, @Description)", connection);
                insertCommand.Parameters.AddWithValue("@Email", email);
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@Description", description);

                var result = await insertCommand.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> UploadVideo(string email, string name, byte[] videoFile)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // check if there is a row in the database with the same email and name
                var checkCommand = new SqlCommand("SELECT COUNT(*) FROM DIYTable WHERE DIYEmail=@Email AND DIYName=@Name", connection);
                checkCommand.Parameters.AddWithValue("@Email", email);
                checkCommand.Parameters.AddWithValue("@Name", name);
                var count = (int)await checkCommand.ExecuteScalarAsync();

                if (count > 0)
                {
                    // if row exists, update it with the video file
                    var command = new SqlCommand("UPDATE DIYTable SET DIYVideo=@Video WHERE DIYEmail=@Email AND DIYName=@Name", connection);
                    command.Parameters.AddWithValue("@Video", videoFile);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Name", name);

                    var result = await command.ExecuteNonQueryAsync();

                    return result > 0;
                }
                else
                {
                    // if row does not exist, return false
                    return false;
                }
            }
        }
        public List<DIYObject> GetDashboardDIY(string email)
        {
            var diyList = new List<DIYObject>();
            var idList = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string getIdQuery = "SELECT DIYID FROM DIYDashboard WHERE DIYEmail = @Email";
                var getIdCommand = new SqlCommand(getIdQuery, connection);
                getIdCommand.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader idReader = getIdCommand.ExecuteReader())
                {
                    while (idReader.Read())
                    {
                        idList.Add(idReader["DIYID"].ToString());
                    }
                }

                // Get the DIYObjects with matching IDs from DIYTable
                if (idList.Count > 0)
                {
                    string getDiyQuery = $"SELECT * FROM DIYTable WHERE DIYID IN ({string.Join(",", idList.Select(id => $"'{id}'"))})";
                    var getDiyCommand = new SqlCommand(getDiyQuery, connection);

                    using (SqlDataReader reader = getDiyCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var diy = new DIYObject
                            {
                                ID = reader["DIYID"].ToString(),
                                Email = reader["DIYEmail"].ToString(),
                                Name = reader["DIYName"].ToString(),
                                Description = reader["DIYDescription"].ToString(),
                            };

                            if (reader["DIYVideo"] != DBNull.Value)
                            {
                                diy.Video = new MemoryStream((byte[])reader["DIYVideo"]);
                            }

                            diyList.Add(diy);
                        }
                    }
                }
            }

            return diyList;
        }
        /*public async Task<bool> ClearDIYTable()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                //This is a test DIY Project description.
                //var command = new SqlCommand("DELETE FROM DIYTable", connection);
                var command = new SqlCommand("DELETE FROM DIYDashboard", connection);
                //var command = new SqlCommand("ALTER TABLE DIYTable ADD DIYID INT IDENTITY(1,1)", connection);
                //var command = new SqlCommand("CREATE TABLE DIYDashboard(DIYEmail NVARCHAR(50) NOT NULL, DIYID NVARCHAR(50) NOT NULL)", connection);

                //var command = new SqlCommand("CREATE TABLE DIYTable (DIYID int IDENTITY(1,1) PRIMARY KEY, DIYEmail VARCHAR(50), DIYName VARCHAR(50), DIYDescription VARCHAR(200), DIYVideo VARBINARY(MAX))", connection);
                //createTableCommand.ExecuteNonQuery();

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }*/

        public List<DIYObject> SearchDIY(string searchTerm)
        {
            var diyList = new List<DIYObject>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM DIYTable WHERE DIYName LIKE '%' + @SearchTerm + '%'";

                var command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@SearchTerm", searchTerm);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var diy = new DIYObject
                        {
                            ID = reader["DIYID"].ToString(),
                            Email = reader["DIYEmail"].ToString(),
                            Name = reader["DIYName"].ToString(),
                            Description = reader["DIYDescription"].ToString(),
                        };

                        if (reader["DIYVideo"] != DBNull.Value)
                        {
                            diy.Video = new MemoryStream((byte[])reader["DIYVideo"]);
                        }

                        diyList.Add(diy);
                    }
                }
            }
            return diyList;
        }
        public async Task<DIYObject> GetDIY(string email, string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SELECT * FROM DIYTable WHERE DIYEmail = @Email AND DIYName = @Name", connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Name", name);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var diy = new DIYObject
                        {
                            ID = reader["DIYID"].ToString(),
                            Email = reader["DIYEmail"].ToString(),
                            Name = reader["DIYName"].ToString(),
                            Description = reader["DIYDescription"].ToString(),
                        };

                        if (reader["DIYVideo"] != DBNull.Value)
                        {
                            diy.Video = new MemoryStream((byte[])reader["DIYVideo"]);
                        }
                        return diy;
                    }
                }
            }
            return null;
        }

        public async Task<bool> AddDIY(string id, string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var verifyCommand = new SqlCommand("SELECT COUNT(*) FROM DIYDashboard WHERE DIYEmail = @Email AND DIYID = @ID", connection);
                verifyCommand.Parameters.AddWithValue("@ID", id);
                verifyCommand.Parameters.AddWithValue("@Email", email);

                var count = (int)await verifyCommand.ExecuteScalarAsync();
                if (count > 0)
                {
                    return false;
                }

                var command = new SqlCommand("INSERT INTO DIYDashboard (DIYID, DIYEmail) VALUES (@ID, @Email)", connection);
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@Email", email);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
