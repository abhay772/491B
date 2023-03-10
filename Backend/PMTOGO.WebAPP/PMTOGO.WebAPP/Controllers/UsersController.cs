using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMTOGO.WebAPP.DAO;
using PMTOGO.WebAPP.Data;
using PMTOGO.WebAPP.Models.Entities;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Globalization;
using ILogger = PMTOGO.WebAPP.Interfaces.ILogger;
using PMTOGO.WebAPP.Interfaces;

namespace PMTOGO.WebAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        //private readonly UsersDbContext usersDbContext;
        private readonly IAuthManager _authManager;
        private readonly ILogger _logger;

        public UsersController(IAuthManager authManager, ILogger logger)
        {
            _authManager = authManager;
            _logger = logger;
        }
#if DEBUG
        [HttpGet]
        [Route("health")]   //make sure controller route works.
        public Task<IActionResult> HealthCheck()
        {
            return Task.FromResult<IActionResult>(Ok("Healthy"));
        }
#endif


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] UserCredentials userCredentials)
        {
            try
            {
                Result result = await _authManager.Login(userCredentials.Username, userCredentials.Password);

                if (result.IsSuccessful)
                {
                    var loginDTO = (LoginDTO)result.Payload!;

                    var sendingOtpResult = await SendOTPtoEmailAsync(loginDTO.Otp!, userCredentials.Username);

                    if (sendingOtpResult.IsSuccessful)
                    {
                        string principalString = JsonSerializer.Serialize(loginDTO.Principal);


                        await SetCookieOptionsAsync(principalString);

                        await SetCorsOptionsAsync();

                        return Ok("Login successfull");
                    }

                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }

                else
                {
                    return BadRequest("Invalid username or password provided. Retry again or contact system admin");

                }
            }

            catch
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

        public class UserCredentials
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
        public class UserRegister 
        {
            public string Email { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;

        }






        //old controller
        /*        //get all users
                [HttpGet]
                public async Task<IActionResult> GetUsers()
                {
                    return Ok(await usersDAO.Users.ToListAsync());
                }

                [HttpGet]
                [Route("{id:Guid}")]
                [ActionName("GetUserById")]
                public async Task<IActionResult> GetUserById([FromRoute] Guid id)
                {
                    await usersDbContext.User.FirstOrDefaultAsync(x => x.Id == id);

                    var user = await usersDbContext.User.FindAsync(id);

                    if (user == null)
                    {
                        return NotFound();
                    }
                    return Ok(user);
                }

                [HttpGet]
                [Route("{email}")]
                [ActionName("GetUserByEmail")]
                public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
                {
                    await usersDbContext.User.FirstOrDefaultAsync(x => x.Email == email);

                    var user = await usersDbContext.User.FindAsync(email);

                    if (user == null)
                    {
                        return NotFound();
                    }
                    return Ok(user);
                }


                [HttpPost]                      //class that match the binding UserProfile/UserAccount?
                public async Task<IActionResult> AddUser(User user)
                {
                    user.Id = Guid.NewGuid();
                    await usersDbContext.User.AddAsync(user);
                    await usersDbContext.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

                    /*curl -i -X POST https://localhost:7079/account/data 
                }

                [HttpPut]
                [Route("{id:Guid}")]
                public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] User updateUser)
                {
                    var user = await usersDbContext.User.FindAsync(id);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.PassDigest = updateUser.PassDigest;
                    user.Email = updateUser.Email;

                    await usersDbContext.SaveChangesAsync();

                    return Ok(user);
                }

                [HttpGet]
                [Route("{id:Guid")]
                public Task<List<User>> GatherUsers ([FromRoute] Guid id)
                {
                    var user = usersDbContext.User.FindAsync(id);
                    return Task.Run(() => manager.GetList());
                }

                [HttpDelete]
                [Route("{id:Guid}")]

                public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
                {
                    var user = await usersDbContext.User.FindAsync(id);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    usersDbContext.User.Remove(user);
                    await usersDbContext.SaveChangesAsync();

                    return Ok();
                }*/
    }
}

