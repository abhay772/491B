using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System;
using System.Globalization;

namespace AA.PMTOGO.Services
{
    //input validation, error handling , logging
    public class UsageAnalysisDashboard : IUsageAnalysisDashboard
    {
        LoggerDAO _logger = new LoggerDAO();
        public async Task<Result> GenerateAnalysis()
        {
            Result result = new Result();
            List<Object> analysis = new List<Object>();
            try
            {
                Result Registrations = await GetRegistrationsPerDay();
                IDictionary<DateTime, int> registrations = (IDictionary<DateTime, int>)Registrations.Payload!;

                Result Logins = await GetLoginsPerDay();
                IDictionary<DateTime, int> logins = (IDictionary<DateTime, int>)Logins.Payload!;

                List<DateTime> dates = GetDates();
                List<string> date = new List<string>();
                foreach (DateTime day in dates)
                {
                    string formattedDate = day.ToString("dd/MM");
                    date.Add(formattedDate);
                    
                }
                analysis.Add(dates);
                analysis.Add(date);
                IDictionary<DateTime, int> registrations_counts = CheckData(registrations);
                analysis.Add(registrations);
                IDictionary<DateTime, int> logins_counts = CheckData(logins);
                analysis.Add(logins);

                result.IsSuccessful = true;
                result.Payload = analysis;
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Generate Analysis Unsuccessful";

            }
            return result;
        }


        public async Task<Result> GetLoginsPerDay()
        {
            Result logins = await _logger.GetAnalysisLogs("Authenticate");
            return logins;
        }

        public async Task<Result> GetRegistrationsPerDay()
        {
            Result registrations = await _logger.GetAnalysisLogs("CreateAccount");
            return registrations;
        }

        private List<DateTime> GetDates()
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime today = DateTime.Today;
            DateTime threeMonthsAgo = today.AddMonths(-3);
            for (DateTime date = threeMonthsAgo; date <= today; date = date.AddDays(1))
            {
                dates.Add(date);
            }
            return dates;
        }

        private IDictionary<DateTime, int> CheckData(IDictionary<DateTime, int> operation_data)
        {
            List<DateTime> dates = GetDates();
            foreach (DateTime dateTime in dates)
            {
                if (!operation_data.ContainsKey(dateTime))
                {
                    operation_data.Add(dateTime, 0);
                }
            }
            return operation_data;

        }

    }
}
