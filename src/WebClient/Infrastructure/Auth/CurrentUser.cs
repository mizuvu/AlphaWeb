using Application.Contracts.Authorization;
using System.Security.Claims;

namespace WebClient.Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId =>
        _httpContextAccessor.HttpContext?.User?.GetUserId() ?? string.Empty;

    public string UserName =>
        _httpContextAccessor.HttpContext?.User?.GetUserName() ?? string.Empty;

    public string FirstName =>
        _httpContextAccessor.HttpContext?.User?.GetFirstName() ?? string.Empty;

    public string LastName =>
        _httpContextAccessor.HttpContext?.User?.GetLastName() ?? string.Empty;

    public string FullName => LastName + " " + FirstName;

    public string PhoneNumber =>
        _httpContextAccessor.HttpContext?.User?.GetPhoneNumber() ?? string.Empty;

    public string Email =>
        _httpContextAccessor.HttpContext?.User?.GetEmail() ?? string.Empty;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated is true;

    public string AccessToken =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(AppClaimType.AccessToken)
        ?? throw new UnauthorizedAccessException("Token is not available.");

    public bool IsInRole(string role) =>
        _httpContextAccessor.HttpContext?.User?.IsInRole(role) is true;

    public bool HasPermission(string permission) =>
        _httpContextAccessor.HttpContext?.User?
        .HasClaim(AppClaimType.Permission, permission) is true
        || DefaultUser.MASTER_USERS.Any(x => x == UserName);
}
