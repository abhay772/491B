using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;

namespace AA.PMTOGO.DAL;


public class SqlPropEvalDAO : ISqlPropEvalDAO
{
    private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.PropertyProfilesDB;Trusted_Connection=True;Encrypt=false";

    public async Task<Result> loadProfileAsync(string username)
    {
        Result result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT * FROM PropertyProfiles WHERE @Username = username";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@Username", username);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                try
                {
                    reader.Read();

                    if (reader != null)
                    {
                        var propertyProfile = new PropertyProfile();

                        propertyProfile.NoOfBedrooms = (int)reader["NoOfBedrooms"];
                        propertyProfile.NoOfBathrooms = (int)reader["NoOfBathrooms"];
                        propertyProfile.SqFeet = (int)reader["SqFeet"];
                        propertyProfile.Address1 = (string)reader["Address1"];
                        propertyProfile.Address2 = (string)reader["Address2"];
                        propertyProfile.City = (string)reader["City"];
                        propertyProfile.State = (string)reader["State"];
                        propertyProfile.Zip = (string)reader["Zip"];
                        propertyProfile.Description = (string)reader["Description"];

                        result.IsSuccessful = true;
                        result.Payload = propertyProfile;
                        return result;
                    }
                }
                catch
                {

                    result.ErrorMessage = "There was an unexpected server error. Please try again later.";
                    result.IsSuccessful = false;
                    //_logger!.Log("FindUser", 4, LogCategory.Server, result);

                }
            }
        }
        result.IsSuccessful = false;
        result.ErrorMessage = "No Property profile.";
        return result;
    }


    public async Task<Result> saveProfileAsync(string username, PropertyProfile propertyProfile)
    {
        Result result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = saveProfileSQLProcedure();

            var command = new SqlCommand(sqlQuery, connection);
            //  @NoOfBedrooms, @NoOfBathrooms, @SqFeet, @Address1, @Address2, @City, @State, @Zip, @Description

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@NoOfBedrooms", propertyProfile.NoOfBedrooms);
            command.Parameters.AddWithValue("@NoOfBathrooms", propertyProfile.NoOfBathrooms);
            command.Parameters.AddWithValue("@SqFeet", propertyProfile.SqFeet);
            command.Parameters.AddWithValue("@Address1", propertyProfile.Address1);
            command.Parameters.AddWithValue("@Address2", propertyProfile.Address2);
            command.Parameters.AddWithValue("@City", propertyProfile.City);
            command.Parameters.AddWithValue("@State", propertyProfile.State);
            command.Parameters.AddWithValue("@Zip", propertyProfile.Zip);
            command.Parameters.AddWithValue("@Description", propertyProfile.Description);

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
            catch
            {
                result.ErrorMessage = "There was an unexpected SQL error. Please try again later.";
                result.IsSuccessful = false;
                //_logger!.Log("FindUser", 4, LogCategory.Server, result);

            }
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "No Property profile.";
        return result;
    }


    public async Task<Result> updatePropEval(string username, int evalPrice)
    {
        Result result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = @"INSERT INTO PropertyProfiles (Username, PropertyEvaluation)
                                VALUES (@Username, @PropertyEvaluation)";

            var command = new SqlCommand(sqlQuery, connection);
            //  @NoOfBedrooms, @NoOfBathrooms, @SqFeet, @Address1, @Address2, @City, @State, @Zip, @Description

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@PropertyEvaluation", evalPrice);

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
            catch
            {
                result.ErrorMessage = "There was an unexpected SQL error. Please try again later.";
                result.IsSuccessful = false;
                //_logger!.Log("FindUser", 4, LogCategory.Server, result);

            }
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "No Property profile.";
        return result;
    }

    public static string saveProfileSQLProcedure()
    {
        return @"IF NOT EXISTS(SELECT * FROM PropertyProfiles WHERE Username = @Username)
                BEGIN
                    INSERT INTO PropertyProfiles (Username, NoOfBedrooms, NoOfBathrooms, SqFeet, Address1, Address2, City, State, Zip, Description) 
                    VALUES (@Username, @NoOfBedrooms, @NoOfBathrooms, @SqFeet, @Address1, @Address2, @City, @State, @Zip, @Description)
                END
                ELSE
                BEGIN
                    UPDATE PropertyProfiles SET NoOfBedrooms = @NoOfBedrooms, NoOfBathrooms = @NoOfBathrooms, SqFeet = @SqFeet, 
                    Address1 = @Address1, Address2 = @Address2, City = @City, State = @State, Zip = @Zip, Description = @Description 
                    WHERE Username = @Username
                END;";
    }

}