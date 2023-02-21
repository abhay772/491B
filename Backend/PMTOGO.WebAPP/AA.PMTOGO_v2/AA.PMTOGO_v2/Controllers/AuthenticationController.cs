using AA.PMTOGO.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace AA.PMTOGO_v2.Controllers;

public class Authentication : ControllerBase
{
    private readonly Authenticator _authManager = new Authenticator();

    [HttpPost("Login")]
    [Consumes("application/json")]
    public async Task<IActionResult> Login([FromBody] UserCredentials userCredentials)
    {
        LoginDTO loginDTO = _authManager.Authenticate(userCredentials.Username, userCredentials.Password);
        //Response.Cookies.Append("authCookie");
        return await Task.FromResult(Ok("You have successfully logged in!"));
    }

    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginDTO
    {
        public string otp { get; set; }

        public IPrincipal principal { get; set; }

    }
}
