using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System.Diagnostics;

namespace AA.PMTOGO.Managers
{
    public class CrimeMapManager : ICrimeMapManager
    {
        private readonly ICrimeMapService _crimeMapService;
        private readonly ILogger? _logger;
        private Result _result;
        private CrimeAlert _alert;

        public CrimeMapManager(ICrimeMapService crimeMap, ILogger logger, Result result, CrimeAlert alert)
        {
            _crimeMapService = crimeMap;
            _logger = logger;
            _result = result;
            _alert = alert;
        }

        public async Task<Result> AddCrimeAlert(string email, string name, string description, string time, string date, string x, string y)
        {
            _result = await _crimeMapService.CheckAlert(email);

            if (_result.IsSuccessful == true)
            {
                _alert = CreateAlert(email, name, description, time, date, x, y);
                _result = await _crimeMapService.AddAlert(_alert);
            }
            return _result;
        }

        public async Task<Result> DeleteCrimeAlert(string email, string id)
        {
            _result = await _crimeMapService.DeleteAlert(email, id);
            return _result;
        }

        public async Task<Result> EditCrimeAlert(string email, string id, string name, string description, string time, string date, string x, string y)
        {
            _alert = CreateAlert(email, id, name, description, time, date, x, y);
            _result = await _crimeMapService.EditAlert(email, id);
            return _result;
        }

        public async Task<CrimeAlert> ViewCrimeAlert(string email, string id) 
        {
            _alert = await _crimeMapService.ViewAlert(email, id);
            return _alert;
        }

        public async Task<List<CrimeAlert>> GetCrimeAlerts()
        {
            var crimeAlerts = new List<CrimeAlert>();
            crimeAlerts = await _crimeMapService.GetAlerts();
            return crimeAlerts;
        }

        public CrimeAlert CreateAlert(string email, string name, string description, string time, string date, string x, string y)
        {
            //validate here
            _alert.Email = email;
            _alert.Name = name;
            _alert.Description = description;
            _alert.Time = time;
            _alert.Date = date;
            _alert.X = x;
            _alert.Y = y;
            return _alert;
        }
        public CrimeAlert CreateAlert(string email, string id, string name, string description, string time, string date, string x, string y)
        {
            //validate here
            _alert.Email = email;
            _alert.Name = name;
            _alert.Description = description;
            _alert.Time = time;
            _alert.Date = date;
            _alert.X = x;
            _alert.Y = y;
            return _alert;
        }
    }
}
