using Infrastructure.Identity.Abstractions;
using Shared.Authorization;
using System.Security.Claims;

namespace Infrastructure.Identity.Services
{
    public class CustomAuthDataProvider : IAuthDataProvider
    {
        public string Permission => AppClaimType.Permission;

        public string UserId => AppClaimType.UserId;

        public string UserName => AppClaimType.UserName;

        public string FirstName => AppClaimType.FirstName;

        public string LastName => AppClaimType.LastName;

        public string FullName => AppClaimType.FullName;

        public string PhoneNumber => AppClaimType.PhoneNumber;

        public string Email => AppClaimType.Email;

        public string Role => AppClaimType.Role;

        public string ImageUrl => nameof(ImageUrl);

        public string Expiration => AppClaimType.Expiration;

        public string AccessToken => AppClaimType.AccessToken;

        public string NewId() => MassTransit.NewId.Next().ToString();

        public IEnumerable<Claim> GetAllClaims => ClaimsExtensions.AppClaims;

        public IEnumerable<string> MasterUsers => DefaultUser.MASTER_USERS;
    }
}
