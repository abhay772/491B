

using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Services;

public class PriceChartAccessor : IPriceChartAccessor
{
    private readonly IPriceChartDAO _priceChartDAO;

    public PriceChartAccessor(IPriceChartDAO priceChartDAO)
    {
        _priceChartDAO = priceChartDAO;
    }

    public async Task<Result> GetItems(int PageNumber, int PageSizez)
    {
        // log here

        Result result = await _priceChartDAO.GetItems(PageNumber, PageSizez);

        return result;
    }

    public async Task<Result> GetChartData(int itemID, int time)
    {

        Result result;
        // log here

        int days;

        if (time == 0)
        {
            days = 7;
        }
        else if (time == 1)
        {
            days = 30;
        }
        else if (time == 2)
        {
            days = 365;
        }

        else
        {
            result = new Result()
            {
                IsSuccessful = false,
                ErrorMessage = "Invalid time span"
            };

            return result;
        }

        result = await _priceChartDAO.GetChartData(itemID, time);

        if (result.IsSuccessful)
        {
            List<ChartData> chartData = (List<ChartData>)result.Payload!;

            DateOnly currentDate = chartData[0].Date;

            int index = 0;
            List<PriceChartDataDTO> priceChartDataDTOs = new List<PriceChartDataDTO>();

            while (chartData.Last().Date > currentDate)
            {
                DateOnly endDate = currentDate.AddDays(days);

                double sum = 0;
                int count = 0;

                while (index < chartData.Count && chartData[index].Date <= endDate)
                {
                    sum += chartData[index].Price;

                    count++;
                    index++;
                }

                double avg = count > 0 ? sum / count : 0;

                currentDate = endDate;

                PriceChartDataDTO priceChartDataDTO = new PriceChartDataDTO()
                {
                    endDate = endDate,
                    AvgPrice = avg,
                };

                priceChartDataDTOs.Add(priceChartDataDTO);
            }

            result.IsSuccessful = true;
            result.Payload = priceChartDataDTOs;
            return result;
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "Unable to generate data";
        return result;
    }
}
