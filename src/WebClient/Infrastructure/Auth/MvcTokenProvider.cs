using Application.Contracts.Authorization;
using System.Security.Claims;

namespace WebClient.Infrastructure.Auth;

public class MvcTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MvcTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetToken()
    {
        var token = _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaimType.AccessToken);

        ArgumentNullException.ThrowIfNull(token);

        return token;
    }
}