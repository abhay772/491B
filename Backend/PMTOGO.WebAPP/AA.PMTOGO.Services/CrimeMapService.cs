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

        public async Task<Result> CheckAlert(string email)
        {

            return _result;
        }

    }
}
