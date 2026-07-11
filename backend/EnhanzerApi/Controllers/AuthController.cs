using EnhanzerApi.DTOs;
using EnhanzerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnhanzerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IExternalAuthService _externalAuth;
    private readonly ITokenService _tokenService;
    private readonly ILocationService _locationService;

    public AuthController(IExternalAuthService externalAuth, ITokenService tokenService, ILocationService locationService)
    {
        _externalAuth = externalAuth;
        _tokenService = tokenService;
        _locationService = locationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _externalAuth.LoginAsync(request.Email, request.Password);

        if (!result.Success)
            return Unauthorized(new AuthErrorDto { Message = result.ErrorMessage ?? "Invalid email or password." });

        // Company_Code was set to the user's email when calling the external API,
        // so we key the saved locations by that same value.
        await _locationService.SaveLocationsAsync(request.Email, result.Locations);

        var (token, expiresAtUtc) = _tokenService.GenerateToken(request.Email);

        return Ok(new LoginResponseDto
        {
            Token = token,
            ExpiresAtUtc = expiresAtUtc,
            Email = request.Email,
            Locations = result.Locations
        });
    }
}
