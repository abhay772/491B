
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace AA.PMTOGO.DAL;

public class PriceChartDAO : IPriceChartDAO
{
    private readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.PriceChartDB;Trusted_Connection=True;Encrypt=false";

    public PriceChartDAO()
    {

    }
    public async Task<Result> GetItems(int PageNumber, int PageSize)
    {
        Result result = new Result();
        int offset = (PageNumber - 1) * PageSize;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT * FROM PriceItems OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using (var command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Offset", offset);
                command.Parameters.AddWithValue("@PageSize", PageSize);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    List<ChartItems> items = new List<ChartItems>();

                    while (await reader.ReadAsync())
                    {
                        ChartItems item = new ChartItems
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString()!,
                            Price = Convert.ToDouble(reader["Price"])
                        };

                        items.Add(item);
                    }
                    result.Payload = items;
                    result.IsSuccessful = true;
                    return result;
                }
             
            }
            
        }
        
    }

    public async Task<Result> GetChartData(int itemID, int time)
    {
        Result result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "Select * FROM PriceData Where ItemID = ItemID ORDER BY Date ASC";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@ItemID", itemID);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                List<ChartData> ChartData = new List<ChartData>();

                while (await reader.ReadAsync())
                {
                    ChartData data = new ChartData
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Date = DateOnly.Parse(reader["Date"].ToString()!),
                        Price = Convert.ToDouble(reader["Price"])
                    };

                    ChartData.Add(data);
                }
                result.Payload = ChartData;
                result.IsSuccessful = true;
                return result;
            }

            /*result.ErrorMessage = "No Item Data was found";
            result.IsSuccessful = false;
            return result;*/
        }
    }
}
