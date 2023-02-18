using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Web.Services
{
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public AppUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim(AppClaimType.UserId, user.Id ?? "anonymous"));
            identity.AddClaim(new Claim(AppClaimType.UserName, user.UserName ?? string.Empty));
            identity.AddClaim(new Claim(AppClaimType.FirstName, user.FirstName ?? string.Empty));
            identity.AddClaim(new Claim(AppClaimType.LastName, user.LastName ?? string.Empty));
            identity.AddClaim(new Claim(AppClaimType.FullName, user.LastName + " " + user.FirstName ?? string.Empty));
            identity.AddClaim(new Claim(AppClaimType.PhoneNumber, user.PhoneNumber ?? string.Empty));
            identity.AddClaim(new Claim(AppClaimType.Email, user.Email ?? string.Empty));
            return identity;
        }
    }
}