namespace WebClient.Infrastructure.Auth;

public interface ICurrentUser
{
    string UserId { get; }
    string UserName { get; }
    string FullName { get; }
    string PhoneNumber { get; }
    string Email { get; }
    string AccessToken { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
    bool HasPermission(string permission);
}
