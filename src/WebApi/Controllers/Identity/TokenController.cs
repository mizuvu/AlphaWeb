using Application.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Identity;

namespace WebApi.Controllers.Identity;

[AllowAnonymous]
[ApiController]
[Route("api/v{version:apiVersion}/oauth")]
public class TokenController : VersionedApiControllerBase
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