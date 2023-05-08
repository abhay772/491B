using AA.PMTOGO.Models.Entities;
using System.Net.Mail;
using System.Net;
using AA.PMTOGO.Services.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Libary;

namespace AA.PMTOGO.Services
{
    public class CrimeMapService : ICrimeMapService
    {
        private readonly AutomaticEmail _emailer;
        private readonly ILogger _logger;
        private readonly ICrimeMapDAO _mapDAO;
        private Result _result;

        public CrimeMapService(AutomaticEmail emailer, ILogger logger, ICrimeMapDAO mapDAO, Result result)
        {
            _emailer = emailer;
            _logger = logger;
            _mapDAO = mapDAO;
            _result = result;
        }
        public async Task<Result> AddAlert(CrimeAlert alert)
        {
            _result = await _mapDAO.AddAlert(alert);
            if(_result.IsSuccessful == true)
            {
                await _emailer.EmailNotification(alert.Email, "New Alert Made", "Your crime alert was created.");
            }
            return _result;
        }
        public async Task<Result> CheckAlert(string email)
        {
            _result = await _mapDAO.CheckAlert(email);
            return _result;
        }
        public async Task<Result> DeleteAlert(string email, int id)
        {
            _result = await _mapDAO.DeleteAlert(email, id);
            if (_result.IsSuccessful == true)
            {
                await _emailer.EmailNotification(email, "Alert Deleted", "Your crime alert was deleted.");
            }
            return _result;
        }
        public async Task<Result> EditAlert(string email, int id, CrimeAlert alert)
        {
            _result = await _mapDAO.EditAlert(email, id, alert);
            return _result;
        }
        public async Task<List<CrimeAlert>> GetAlerts()
        {
            var crimeAlerts = new List<CrimeAlert>();
            crimeAlerts = await _mapDAO.GetAlerts();
            return crimeAlerts;
        }
    }
}
