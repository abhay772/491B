using Microsoft.AspNetCore.Mvc;
using PMTOGO.WebAPP.LibAccount;
using PMTOGO.WebAPP.Models.Entities;
using System.Security.Claims;
using System.Security.Principal;

namespace PMTOGO.WebAPP.Managers
{
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

            string role = null!;

            if (result.IsSuccessful)
            {
                role = (string)result.Payload!; //null forgiving
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
            loginDTO.Principal = principal;
            loginDTO.Otp = otp;

            result.IsSuccessful = true;
            result.ErrorMessage = string.Empty;
            result.Payload = loginDTO;

            return result;
        }

        public async Task<Result> RegisterUser(string email, string password, string firstname, string lastname, string role)
        {

            Result result = await Task.Run(() => _authenticator.CreateUser(email, password, firstname, lastname, role));
            return result;
        }
    }

}

