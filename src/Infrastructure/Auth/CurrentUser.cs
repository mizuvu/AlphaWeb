using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Authorization;
using System.Security.Claims;

namespace Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string UserId => User?.GetUserId() ?? string.Empty;

    public string UserName => User?.GetUserName() ?? string.Empty;

    public string FirstName => User?.GetFirstName() ?? string.Empty;

    public string LastName => User?.GetLastName() ?? string.Empty;

    public string FullName => User?.GetFullName() ?? string.Empty;

    public string PhoneNumber => User?.GetPhoneNumber() ?? string.Empty;

    public string Email => User?.GetEmail() ?? string.Empty;

    public bool IsAuthenticated => User?.IsAuthenticated() is true;

    public bool IsMasterUser => DefaultUser.MASTER_USERS.Any(x => x == UserName);

    public bool IsInRole(string role) => User?.IsInRole(role) is true;

    public bool HasPermission(string permission) =>
        User?.HasPermission(AppClaimType.Permission, permission) is true
        || DefaultUser.MASTER_USERS.Any(x => x == UserName);
}
