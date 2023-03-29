using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Libary;
using AA.PMTOGO.Models.Entities;
using System.Data;
using System.Net;
using System.Net.Mail;


namespace AA.PMTOGO.Managers;

public class PropEvalManager : IPropEvalManager
{
    private readonly ISqlPropEvalDAO _sqlPropEvalDAO;
    private readonly IPropertyEvaluator _evaluator;
    private readonly InputValidation _inputeValidation;

    public PropEvalManager(ISqlPropEvalDAO sqlPropEvalDAO, IPropertyEvaluator evaluator)
    {
        _sqlPropEvalDAO = sqlPropEvalDAO;
        _evaluator = evaluator;
        _inputeValidation = new InputValidation();
    }

    public async Task<Result> loadProfileAsync(string username)
    {
        bool Validation = _inputeValidation.ValidateEmail(username).IsSuccessful;
        if (Validation)
        {
            return await _sqlPropEvalDAO.loadProfileAsync(username);
        }

        Result result = new Result();

        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid Username" ;

        return result;
    }

    public async Task<Result> saveProfileAsync(string username, PropertyProfile propertyProfile)
    {
        bool Validation = _inputeValidation.ValidateEmail(username).IsSuccessful && _inputeValidation.ValidatePropertyProfile(propertyProfile);
        if (Validation)
        {
            return await _sqlPropEvalDAO.saveProfileAsync(username, propertyProfile);
        }

        Result result = new Result();

        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid Username or Property Profile";

        return result;
    }

    public async Task<Result> evaluateAsync(string username, PropertyProfile propertyProfile)
    {
        Result result = new Result();

        try
        {
            bool Validation = _inputeValidation.ValidateEmail(username).IsSuccessful && _inputeValidation.ValidatePropertyProfile(propertyProfile);
            if (!Validation)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid Username";

                return result;
            }
            Result evaluationResult = await _evaluator.evaluate(propertyProfile);

            if (result.IsSuccessful)
            {
                // Updating the profile with the evaluation
                Result saveEvalResult = await _sqlPropEvalDAO.updatePropEval(username, (int)evaluationResult.Payload!);

                // Sending an email notifying the user, that their evaluation is ready.

                Task<Result> sendNotificationtoEmailAsync = SendNotificationtoEmailAsync(username);

                // this is to make that the task does not take more than 5 secs
                Task taskDone = await Task.WhenAny(sendNotificationtoEmailAsync, Task.Delay(TimeSpan.FromSeconds(5)));

                if(taskDone == sendNotificationtoEmailAsync)
                {
                    Result notificationResult = await sendNotificationtoEmailAsync;

                    // log the result
                }

                else
                {
                    // log the error
                }

                return evaluationResult;
            }

            return evaluationResult;
        }

        catch (Exception ex)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = ex.Message;
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "There was an error with Evaluating.";

        return result;
    }

    private async Task<Result> SendNotificationtoEmailAsync(string userEmail)
    {
        Result result = new Result();

        string fromEmail = "aa.pmtogo.otp@gmail.com";
        string toEmail = userEmail;
        string subject = "Property Evaluation";
        string body = "You Property Evaluation is ready, and can be viewd on PMTOGO.com";
        string password = "017535386";

        try
        {
            using (var message = new MailMessage(fromEmail, toEmail))
            {
                message.Subject = subject;
                message.Body = body;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(fromEmail, password);

                    await smtpClient.SendMailAsync(message);
                }
            }

            result.IsSuccessful = true;
        }

        catch
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Email was not sent.";
        }

        return result;
    }
}
