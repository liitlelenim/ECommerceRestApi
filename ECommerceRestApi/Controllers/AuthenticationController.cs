using ECommerceRestApi.DTO.Authentication;
using ECommerceRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceRestApi.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Registers account with data given in registration dto.
    /// </summary>
    /// <param name="registrationDto"></param>
    /// <returns>No content response on successful operation.</returns>
    [HttpPost, Route("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> RegisterAccount([FromBody] UserRegistrationDto registrationDto)
    {
        try
        {
            await _authenticationService.Register(registrationDto);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
    /// <summary>
    /// Make an attempt to login with dto data.
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns>User JWT token on successful login attempt.</returns>
    [HttpPost, Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        try
        {
            return Ok(await _authenticationService.Login(loginDto));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}