using Application.Contracts.Identity;
using Application.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/v{version:apiVersion}/oauth")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest request)
    {
        return Ok(await _tokenService.GetTokenAsync(request));
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        return Ok(await _tokenService.RefreshTokenAsync(request));
    }
}