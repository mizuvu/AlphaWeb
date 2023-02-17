namespace Infrastructure.Identity.Abstractions
{
    public interface IClaimType
    {
        string Permission { get; }
        string UserId { get; }
        string UserName { get; }
        string FirstName { get; }
        string LastName { get; }
        string FullName { get; }
        string PhoneNumber { get; }
        string Email { get; }
        string Role { get; }
        string ImageUrl { get; }
        string Expiration { get; }
        string AccessToken { get; }
    }
}
