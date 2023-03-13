using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO_v2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthManager _authManager;

    public AuthenticationController(IAuthManager authManager)
    {
        _authManager = authManager;
    }

    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Login([FromBody] UserCredentials userCredentials)
    {
        try
        {
            Result result = await _authManager.Login(userCredentials.Username, userCredentials.Password);

            if (result.IsSuccessful)
            {
                var loginDTO = (LoginDTO)result.Payload;

                //var sendingOtpResult = await SendOTPtoEmailAsync(loginDTO.otp, userCredentials.Username);

                string principalString = JsonSerializer.Serialize(loginDTO.principal);

                await SetCookieOptionsAsync(principalString);

                await SetCorsOptionsAsync();

                return Ok("Login successfull");
            }

            else
            {
                return BadRequest("Invalid username or password provided. Retry again or contact system admin");

            }
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    private async Task SetCookieOptionsAsync(string principalString)
    {
        // Create a new cookie and add it to the response
        Response.Cookies.Append("CredentialCookie", principalString, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(1),
            MaxAge = TimeSpan.FromHours(24)
        });

        await Task.CompletedTask;
    }

    private async Task SetCorsOptionsAsync()
    {
        Response.Headers.Add("Access-Control-Allow-Origin", "https://www.example.com");
        Response.Headers.Add("Access-Control-Max-Age", "86400"); // 24 hours in seconds
        Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        Response.Headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS");
        Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

        await Task.CompletedTask;
    }
    private async Task<Result> SendOTPtoEmailAsync(string otp, string userEmail)
    {
        Result result = new Result();

        string fromEmail = "aa.pmtogo.otp@gmail.com";
        string toEmail = userEmail;
        string subject = "One Time Password";
        string body = "Following is your one-time-password: " + otp;
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
