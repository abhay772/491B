using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web.Mvc;

namespace AA.PMTOGO.Libary
{
    public class ClaimValidation
    {
        InputValidation _inputValidation = new();

        public Result ClaimsValidation(string role, HttpRequest httpRequest)
        {
            Result result = new Result();
            try
            {
                // Loading the cookie from the http request
                var cookieValue = httpRequest.Cookies["CredentialCookie"];

                if (!string.IsNullOrEmpty(cookieValue))
                {
                    var handler = new JwtSecurityTokenHandler();

                    var jwtToken = handler.ReadJwtToken(cookieValue);

                    if (jwtToken == null)
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "Invalid Claims";
                        return result;
                    }

                    var claims = jwtToken.Claims.ToList();
                    Claim usernameClaim = claims[0];
                    Claim roleClaim = claims[1];

                    if (usernameClaim != null && roleClaim != null)
                    {
                        string username = usernameClaim.Value;
                        string userrole = roleClaim.Value;

                        // Check if the role is Property Manager
                        bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;
                        if (validationCheck && userrole == role || role == null)
                        {
                            UserClaims user = new UserClaims(username, userrole);

                            result.IsSuccessful = true;
                            result.Payload = user;
                            return result;
                        }
                    }
                    return result;
                }
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid Claims";
                return result;
            }
            result.IsSuccessful = false;
            return result;
        }

        public Result ClaimsValidation(string role, string jwt)
        {
            Result result = new Result();
            try
            {
                if (!string.IsNullOrEmpty(jwt))
                {
                    var handler = new JwtSecurityTokenHandler();

                    var jwtToken = handler.ReadJwtToken(jwt);

                    if (jwtToken == null)
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "Invalid Claims";
                        return result;
                    }

                    var claims = jwtToken.Claims.ToList();
                    Claim usernameClaim = claims[0];
                    Claim roleClaim = claims[1];

                    if (usernameClaim != null && roleClaim != null)
                    {
                        string username = usernameClaim.Value;
                        string userrole = roleClaim.Value;

                        // Check if the role is Property Manager
                        bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;
                        if (validationCheck && userrole == role || role == null)
                        {
                            UserClaims user = new UserClaims(username, role!);

                            result.IsSuccessful = true;
                            result.Payload = user;
                            return result;
                        }
                    }
                    return result;
                }
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid Claims";
                return result;
            }
            result.IsSuccessful = false;
            return result;
        }
    }
}