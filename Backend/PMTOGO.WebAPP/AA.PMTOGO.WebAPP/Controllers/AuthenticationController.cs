using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text.Json;

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

    [HttpPost("Login")]
    [Consumes("application/json", "application/problem+json")]
    public async Task<IActionResult> Login([FromBody] UserCredentials userCredentials)
    {


        try
        {
            Result result = await _authManager.Login(userCredentials.Username, userCredentials.Password);

            if (result.IsSuccessful)
            {
                var loginDTO = (LoginDTO)result.Payload!;

                //var sendingOtpResult = await SendOTPtoEmailAsync(loginDTO.otp, userCredentials.Username);

                string principalString = JsonSerializer.Serialize(loginDTO.principal);

                SetCookieOptions(principalString);


                return Ok(new { message = "Login successful" });
            }
            else
            {

                return BadRequest(new { message = "Invalid username or password provided. Retry again or contact system admin" });
            }
        }
        catch (ArgumentException ex)
        {

            return BadRequest(new { message = "Already logged in." });
        }
        catch
        {

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost("Logout")]
    [Consumes("application/json")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.ContainsKey("CredentialCookie"))
        {
            Response.Cookies.Delete("CredentialCookie");
            Console.WriteLine(Request.Cookies["CredentialCookie"].ToString());

            return Ok("Logged out successfully");
        }

        else
        {
            return BadRequest("Cookie dosent exist");
        }


    }

    private void SetCookieOptions(string principalString)
    {
        // Create a new cookie and add it to the response
        Response.Cookies.Append("CredentialCookie", principalString, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.Now.AddDays(1),
            MaxAge = TimeSpan.FromHours(24),
            IsEssential = true,
        });
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
