using AA.PMTOGO.Models.Entities;
using System.Security.Cryptography;
using System.Text;
using AA.PMTOGO.Libary;
using AA.PMTOGO.DAL;
using System.Net.Mail;
using System.Net;
using AA.PMTOGO.Services.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.DAL.Interfaces;
using System.Collections.Generic;

namespace AA.PMTOGO.Services
{
    public class CrimeMapService : ICrimeMapService
    {
        private readonly ILogger _logger;
        private readonly ICrimeMapDAO _mapDAO;
        private Result _result;

        public CrimeMapService(ILogger logger, ICrimeMapDAO mapDAO, Result result)
        {
            _logger = logger;
            _mapDAO = mapDAO;
            _result = result;
        }

        
        public async Task<Result> AddAlert(CrimeAlert alert)
        {
            _result = await _mapDAO.AddAlert(alert);
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
        public async Task<CrimeAlert> ViewAlert(int id)
        {
            var alert = new CrimeAlert();
            alert = await _mapDAO.ViewAlert(id);
            return alert;
        }

    }
}
