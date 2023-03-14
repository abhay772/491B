using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Security.Claims;
using AA.PMTOGO.Libary;

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
    }

    [HttpGet("loadProfile")]
    [Consumes("application/json")]
    public async Task<IActionResult> LoadProfile()
    {
        try
        {
            // Setting the cors options for the response
            await SetCorsOptionsAsync();

            // Loading the cookie from the http request
            var cookieValue = Request.Cookies["CredentialCookie"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                
                // Deserializing the claims from the cookie
                var principalString = cookieValue;
                var principal = JsonSerializer.Deserialize<ClaimsPrincipal>(principalString);

                // Load the username and role from the principal
                var usernameClaim = principal.FindFirst(ClaimTypes.Email);
                var roleClaim = principal.FindFirst(ClaimTypes.Role);

                if (usernameClaim != null && roleClaim != null)
                {
                    string Username = usernameClaim.Value;
                    string role = roleClaim.Value;

                    // Check if the role is Property Manager
                    bool validationCheck = _inputValidation.ValidateUsername(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                    if (role != null && validationCheck && role == "Property Manager")
                    {

                        // Call the loadProfile function 

                        Result result = await _propEvalManager.loadProfileAsync(username);

                        if (result.IsSuccessful)
                        {
                            PropertyProfile propertyProfile = (PropertyProfile)result.Payload;

                            return Ok(propertyProfile);
                        }

                        else
                        {
                            return BadRequest(result.ErrorMessage);
                        }
                    }
                }

            }

           return Forbid(); 
        }

        catch (Exception ex)
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
            // Setting the cors options for the response
            await SetCorsOptionsAsync();

            // Loading the cookie from the http request
            var cookieValue = Request.Cookies["CredentialCookie"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                // Deserializing the claims from the cookie
                var principalString = cookieValue;
                var principal = JsonSerializer.Deserialize<ClaimsPrincipal>(principalString);

                // Load the username and role from the principal
                var usernameClaim = principal.FindFirst(ClaimTypes.Email);
                var roleClaim = principal.FindFirst(ClaimTypes.Role);

                if (usernameClaim != null && roleClaim != null)
                {
                    string Username = usernameClaim.Value;
                    string role = roleClaim.Value;

                    // Check if the role is Property Manager
                    bool validationCheck = _inputValidation.ValidateUsername(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

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

            return Forbid();
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("evaluate")]
    [Consumes("application/json")]
    public async Task<IActionResult> Evaluate(PropertyProfile propertyProfile)
    {
        try
        {
            // Setting the cors options for the response
            await SetCorsOptionsAsync();

            // Loading the cookie from the http request
            var cookieValue = Request.Cookies["CredentialCookie"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                // Deserializing the claims from the cookie
                var principalString = cookieValue;
                var principal = JsonSerializer.Deserialize<ClaimsPrincipal>(principalString);

                // Load the username and role from the principal
                var usernameClaim = principal.FindFirst(ClaimTypes.Email);
                var roleClaim = principal.FindFirst(ClaimTypes.Role);

                if (usernameClaim != null && roleClaim != null)
                {
                    string username = usernameClaim.Value;
                    string role = roleClaim.Value;

                    // Check if the role is Property Manager
                    bool validationCheck = _inputValidation.ValidateUsername(username).IsSuccessful && _inputValidation.ValidateRole(role).IsSuccessful;

                    if (role != null && validationCheck && role == "Property Manager")
                    {

                        // Call the evaluate function 
                        Result result = await _propEvalManager.evaluateAsync(username, propertyProfile);
                        double evalPrice = (double)result.Payload;

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

            return Forbid();
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
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
}
