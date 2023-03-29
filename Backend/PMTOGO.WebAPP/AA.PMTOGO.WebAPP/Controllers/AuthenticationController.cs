using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.WebRequestMethods;

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

    [HttpGet("IsLoggedIn")]
    public  IActionResult IsLoggedIn()
    {
        try
        {
            if (Request.Cookies["CredentialCookie"] != null)
            {
                return Ok(true);
            }

            return Ok(false);
        }

        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
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

                string claims_jwt = CreateJWTToken(loginDTO.claims!);

                SetCookieOptions(claims_jwt);

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

    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        if (Request.Cookies.ContainsKey("CredentialCookie"))
        {
            Response.Cookies.Delete("CredentialCookie", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });

            return Ok(true);
        }

        else
        {
            return Ok(false);
        }


    }

    private string CreateJWTToken(List<Claim> claims)
    {

        // Create a new Jwt token containing the claims
        var token = new JwtSecurityToken(
            issuer: "PMTOGO",
            audience: "users",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: null
            );

        // Serializing the token with the claims
        var handler = new JwtSecurityTokenHandler();
        string tokenString = handler.WriteToken(token);

        return tokenString;
    }

    private void SetCookieOptions(string principalString)
    {
        // Create a new cookie and add it to the response
        Response.Cookies.Append("CredentialCookie", principalString, new CookieOptions
        {
            Domain = "localhost",
            Path= "/",
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
