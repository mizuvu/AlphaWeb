using Application.Common.Interfaces;

namespace Migrator.Services;

public class CurrentUser : ICurrentUser
{
    public string UserId => "Migrator";

    public string UserName => "Migrator";

    public string FullName => string.Empty;

    public string PhoneNumber => string.Empty;

    public string Email => string.Empty;

    public bool IsAuthenticated => false;

    public bool IsMasterUser => false;

    public string FirstName => string.Empty;

    public string LastName => string.Empty;

    public bool IsInRole(string role) => false;

    public bool HasPermission(string permission) => false;
}
