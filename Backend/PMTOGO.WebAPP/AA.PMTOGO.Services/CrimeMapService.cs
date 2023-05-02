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
            if(_result.IsSuccessful == true)
            {
                await EmailNotification(alert.Email, "New Alert Made", "Your crime alert was created.");
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
                await EmailNotification(email, "Alert Deleted", "Your crime alert was deleted.");
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
        public async Task<bool> EmailNotification(string userEmail,string emailSubject,string emailBody)
        {
            string companyEmail = "DemonicKhmer@gmail.com";
            string companyEmailKey = "Your Email third party application key. 2 factor authN must be activated for the gmail account";
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(companyEmail, companyEmailKey);
            smtpClient.EnableSsl = true;

            var message = new MailMessage(companyEmail, userEmail, emailSubject, emailBody);

            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email");
            }
            return false;
        }
    }
}
