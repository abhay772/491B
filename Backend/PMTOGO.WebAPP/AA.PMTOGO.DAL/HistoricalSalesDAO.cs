using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Text.Json;

namespace AA.PMTOGO.DAL;

public class HistoricalSalesDAO : IHistoricalSalesDAO
{
    public async Task<List<double>> findSales(PropertyProfile propertyProfile)
    {
        string expectedZip = propertyProfile.Zip;

        string jsonString = await File.ReadAllTextAsync("D:\\Program Files (x86)\\Class Stuff\\Sem11\\491B\\Backend\\PMTOGO.WebAPP\\Data\\MOCK_DATA.json");

        List<HistoricalSale> salesList = JsonSerializer.Deserialize<List<HistoricalSale>>(jsonString)!;

        if(salesList!.Count == 0 )
        {
            return null!;
        }

        List<double> sales = new List<double>();

        foreach(HistoricalSale sale in salesList)
        {
            if(expectedZip == sale.Zip)
            {
                double saleValue = Double.Parse(sale.SalesValue.Substring(1, sale.SalesValue.Length - 1));
                sales.Add(saleValue);
            }
        }
        return sales;
    }
}

public class HistoricalSale
{
    public int NoOfBedrooms { get; set; }
    public int NoOfBathrooms { get; set; }
    public int SqFeet { get; set; }
    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty; 
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string SalesValue { get; set; } = string.Empty;
}
