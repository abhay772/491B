using AA.PMTOGO.Authentication;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Security.Claims;
using System.Security.Principal;

namespace AA.PMTOGO.Managers;

public class AuthManager : IAuthManager
{
    private readonly IAuthenticator _authenticator;

    public AuthManager(IAuthenticator authenticator)
    {
        _authenticator = authenticator;
    }

    public async Task<Result> Login(string username, string password)
    {
        Result result = new Result();

        int attempts = await _authenticator.GetFailedAttempts(username);

        if (attempts >= 3)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Account disabled. Perform account recovery or contact system admin";
            return result;
        }
        else if (attempts < 0)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Username or password is invalid.";
            return result;
        }
        result = await _authenticator.Authenticate(username, password);

        string role = null!;

        if (result.IsSuccessful)
        {
            role = (string)result.Payload!;
        }

        else
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Username or password is invalid.";
            return result;
        }

        _authenticator.ResetFailedAttempts(username);

        string otp = _authenticator.GenerateOTP();

        var claims = new List<Claim>();
       
        claims.Add(new Claim(ClaimTypes.Email, username));
        claims.Add(new Claim(ClaimTypes.Role, role));

        LoginDTO loginDTO = new LoginDTO();
        loginDTO.claims = claims;
        loginDTO.Otp = otp;

        result.IsSuccessful = true;
        result.ErrorMessage = string.Empty;
        result.Payload = loginDTO;

        return result;
    }
}