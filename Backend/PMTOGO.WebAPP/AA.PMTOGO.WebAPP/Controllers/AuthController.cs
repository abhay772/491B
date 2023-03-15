using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Azure;
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
    [Consumes("application/json")]
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

                //SetCookieOptionsAsync(principalString);



                await SetCorsOptionsAsync();

                Response.Cookies.Append("CredentialCookie", principalString, new CookieOptions
                {
                    HttpOnly = true,
                    Domain = "localhost:7135",
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(1),
                    MaxAge = TimeSpan.FromHours(24),
                    IsEssential = true,
                });

                return Ok(Response);
            }

            else
            {
                return BadRequest("Invalid username or password provided. Retry again or contact system admin");

            }
        }

        catch(ArgumentException ex)
        {
            return BadRequest("Already logged in.");
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
        if (Request.Cookies.ContainsKey("CredentialCookie")){

            Request.Cookies["CredentialCookie"].Remove(0);

            return Ok("Logged out successfully");
        }

        else
        {
            return BadRequest("Cookie dosent exist");
        }

    }

    //private string SetCookieOptions(string principalString)
    //{
    //    // Create a new cookie and add it to the response
    //    Response.Cookies.Append("CredentialCookie", principalString, new CookieOptions
    //    {
    //        //HttpOnly = true,
    //        //Secure = true,
    //        Domain = "https://localhost:7135/",
    //        SameSite = SameSiteMode.None,
    //        Expires = DateTime.Now.AddDays(1),
    //        MaxAge = TimeSpan.FromHours(24),
    //        IsEssential= true,
    //    });

    //    var cookieValue = Request.Cookies["CredentialCookie"];

    //    return cookieValue;
    //}

    private async Task SetCorsOptionsAsync()
    {
        Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7135/swagger/");
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
