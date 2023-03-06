﻿using AA.PMTOGO.Authentication;
using AA.PMTOGO.Models;
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

        if (await _authenticator.GetFailedAttempts(username) >= 3)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Account disabled. Perform account recovery or contact system admin";
        }

        result = _authenticator.Authenticate(username, password);

        string role = null;

        if (result.IsSuccessful)
        {
            role = (string)result.Payload;
        }

        _authenticator.ResetFailedAttempts(username);

        string otp = _authenticator.GenerateOTP();

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, username),
        new Claim(ClaimTypes.Role, role)
    };

        IIdentity identity = new ClaimsIdentity(claims);

        IPrincipal principal = new ClaimsPrincipal(identity);

        LoginDTO loginDTO = new LoginDTO();
        loginDTO.principal = principal;
        loginDTO.otp = otp;

        result.IsSuccessful = true;
        result.ErrorMessage = string.Empty;
        result.Payload = loginDTO;

        return result;
    }
}