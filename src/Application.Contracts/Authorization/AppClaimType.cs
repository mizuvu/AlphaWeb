namespace Application.Contracts.Authorization;

public class AppClaimType
{
    public const string Permission = nameof(Permission);
    public const string UserId = "uid";
    public const string UserName = "un";
    public const string FirstName = "first_name";
    public const string LastName = "last_name";
    public const string FullName = "full_name";
    public const string PhoneNumber = "phone_number";
    public const string Email = "email";
    public const string Role = "role";
    public const string ImageUrl = nameof(ImageUrl);
    public const string Expiration = "exp";
    public const string AccessToken = "token";
}