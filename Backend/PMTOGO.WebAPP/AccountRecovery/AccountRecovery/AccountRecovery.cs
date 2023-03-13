using AA.PMTOGO.SqlUserDAO;
using System.Net.Mail;
using System.Net;

public class AccountRecovery
{
    private string _oTP;
    public async Task<bool> Recovery(string email)
    {
        var mailAddress = new System.Net.Mail.MailAddress(email);
        if(mailAddress.Address == email)
        {
            RecoveryDAO usersDAO = new RecoveryDAO();

            if (usersDAO.DoesUserExist(email).IsSuccessful)
            {
                string otp = GenerateOTP();
                usersDAO.SetRecoveryRequest(email);
                return AutomaticEmail(email, otp).Result;
            }
        }
        return false;
    }

    public string GenerateOTP()
    {
        string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.-@";
        Random rand = new Random();
        for (int i = 0; i < 8; i++)
        {
            _oTP += allowedChars[rand.Next(0, allowedChars.Length)];
        }
        Console.WriteLine(_oTP);
        return _oTP;
    }

    public async Task<bool> ValidateOTP(string email, string otp)
    {
        RecoveryDAO usersDAO = new RecoveryDAO();
        if (otp == _oTP)
        {
            usersDAO.SetSuccessfulOTP(email);
            return true;
        }
        return false;
    }

    public async Task<bool> AutomaticEmail(string email, string oTP)
    {
        return true;
        /*var smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential();
        smtpClient.EnableSsl = true;

        var message = new MailMessage("test1@gmail.com", "test2@gmail.com", "Account Recovey - One Time Password", oTP);

        try
        {
            smtpClient.Send(message);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending email");
        }
        return false;*/
    }
}
