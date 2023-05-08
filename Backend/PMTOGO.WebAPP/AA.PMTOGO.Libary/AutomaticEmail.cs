using System.Net.Mail;
using System.Net;


namespace AA.PMTOGO.Libary
{
    public class AutomaticEmail
    {
        public async Task<bool> EmailNotification(string userEmail, string emailSubject, string emailBody)
        {
            string companyEmail = "pmtogo.prod@gmail.com";
            string companyEmailKey = "veaidqtdcxquusea";
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(companyEmail, companyEmailKey);
            smtpClient.EnableSsl = true;
            Console.WriteLine(companyEmail);
            Console.WriteLine(userEmail);
            Console.WriteLine(emailSubject);
            Console.WriteLine(emailBody);
            var message = new MailMessage(companyEmail, userEmail, emailSubject, emailBody);
            try
            {
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
            return false;
        }
    }
}
