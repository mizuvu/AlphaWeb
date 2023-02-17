using System.Security.Claims;

namespace Shared.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal?.FindFirstValue(AppClaimType.UserId);

    public static string? GetUserName(this ClaimsPrincipal principal)
        => principal?.FindFirstValue(AppClaimType.UserName);

    public static string? GetFullName(this ClaimsPrincipal principal)
        => principal?.FindFirstValue(AppClaimType.FullName);

    public static string? GetFirstName(this ClaimsPrincipal principal)
        => principal?.FindFirstValue(AppClaimType.FirstName);

    public static string? GetLastName(this ClaimsPrincipal principal)
        => principal?.FindFirstValue(AppClaimType.LastName);

    public static string? GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AppClaimType.Email);

    public static string? GetPhoneNumber(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AppClaimType.PhoneNumber);

    public static DateTimeOffset GetExpiration(this ClaimsPrincipal principal) =>
        DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(
            principal.FindFirstValue(AppClaimType.Expiration)));

    private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
        principal is null
            ? throw new ArgumentNullException(nameof(principal))
            : principal.FindFirst(claimType)?.Value;

    public static bool IsAuthenticated(this ClaimsPrincipal principal) =>
        principal.Identity!.IsAuthenticated is true;

    public static bool HasPermission(this ClaimsPrincipal principal, string claimType, string claimValue) =>
        principal.HasClaim(claimType, claimValue) is true;
}