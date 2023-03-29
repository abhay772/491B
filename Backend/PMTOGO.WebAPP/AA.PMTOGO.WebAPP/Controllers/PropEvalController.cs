using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Security.Claims;
using AA.PMTOGO.Libary;
using System.IdentityModel.Tokens.Jwt;

namespace AA.PMTOGO.WebAPP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropEvalController : ControllerBase
{
    private readonly IPropEvalManager _propEvalManager;
    private readonly InputValidation _inputValidation;

    public PropEvalController(IPropEvalManager propEvalManager)
    {
        _propEvalManager = propEvalManager;
        _inputValidation = new InputValidation();
    }

    [HttpGet("loadProfile")]
    [Consumes("application/json")]
    public async Task<IActionResult> LoadProfile()
    {
        try
        {
            // Loading the cookie from the http request
            var cookieValue = Request.Cookies["CredentialCookie"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                var handler = new JwtSecurityTokenHandler();

                var jwtToken = handler.ReadJwtToken(cookieValue);

                if (jwtToken == null)
                {
                    return BadRequest("Invalid Claims");
                }

                var claims = jwtToken.Claims.ToList();
                Claim usernameClaim = claims[0];
                Claim roleClaim = claims[1];
                

                if (usernameClaim != null && roleClaim != null)
                {
                    string username = usernameClaim.Value;
                    string role = roleClaim.Value;

                    // Check if the role is Property Manager
                    bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                    if (role != null && validationCheck && role == "Property Manager")
                    {

                        // Call the loadProfile function 

                        Result result = await _propEvalManager.loadProfileAsync(username);

                        if (result.IsSuccessful)
                        {
                            PropertyProfile propertyProfile = (PropertyProfile)result.Payload!;

                            return Ok(propertyProfile);
                        }

                        else
                        {
                            return BadRequest(result.ErrorMessage);
                        }
                    }
                }

            }

           return BadRequest("Not Authorized"); 
        }

        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("saveProfile")]
    [Consumes("application/json")]
    public async Task<IActionResult> SaveProfile(PropertyProfile propertyProfile)
    {
        try
        {
            // Loading the cookie from the http request
            var cookieValue = Request.Cookies["CredentialCookie"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                var handler = new JwtSecurityTokenHandler();

                var jwtToken = handler.ReadJwtToken(cookieValue);

                if (jwtToken == null)
                {
                    return BadRequest("Invalid Claims");
                }

                var claims = jwtToken.Claims.ToList();
                Claim usernameClaim = claims[0];
                Claim roleClaim = claims[1];

                if (usernameClaim != null && roleClaim != null)
                {
                    string username = usernameClaim.Value;
                    string role = roleClaim.Value;

                    // Check if the role is Property Manager
                    bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                    if (role != null && validationCheck && role == "Property Manager")
                    {

                        // Call the saveProfile function 
                        Result result = await _propEvalManager.saveProfileAsync(username, propertyProfile);

                        if (result.IsSuccessful)
                        {
                            return Ok("Profile saved successfully.");
                        }

                        else
                        {
                            return BadRequest(result.ErrorMessage);
                        }
                    }
                }

            }

            return BadRequest("Cookie not found");
        }

        catch 
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("evaluate")]
    public async Task<IActionResult> Evaluate()
    {
        try
        {
            // Loading the cookie from the http request
            var cookieValue = Request.Cookies["CredentialCookie"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                var handler = new JwtSecurityTokenHandler();

                var jwtToken = handler.ReadJwtToken(cookieValue);

                if (jwtToken == null)
                {
                    return BadRequest("Invalid Claims");
                }

                var claims = jwtToken.Claims.ToList();
                Claim usernameClaim = claims[0];
                Claim roleClaim = claims[1];

                if (usernameClaim != null && roleClaim != null)
                {
                    string username = usernameClaim.Value;
                    string role = roleClaim.Value;

                    // Check if the role is Property Manager
                    bool validationCheck = _inputValidation.ValidateEmail(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                    if (role != null && validationCheck && role == "Property Manager")
                    {

                        Result result = await _propEvalManager.loadProfileAsync(username);

                        if (result.IsSuccessful == false)
                        { 
                            return BadRequest(result.ErrorMessage);
                        }

                        PropertyProfile propertyProfile = (PropertyProfile)result.Payload!;

                        // Call the evaluate function 

                        result = await _propEvalManager.evaluateAsync(username, propertyProfile);

                        if (result.IsSuccessful == false)
                        {
                            return Ok("00000.00");
                        }

                        string evalPrice = result.Payload.ToString();

                        if (result.IsSuccessful)
                        {
                            return Ok(evalPrice);
                        }

                        else
                        {
                            return BadRequest(result.ErrorMessage);
                        }
                    }
                }

            }

            return BadRequest("Not Authorized");
        }

        catch 
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
