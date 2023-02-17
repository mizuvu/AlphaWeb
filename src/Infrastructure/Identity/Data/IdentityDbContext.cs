using Infrastructure.Identity.Abstractions;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Identity.Data;

public abstract class IdentityDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IIdentityDbContext
{
    public IdentityDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<ApplicationUserRole> ApplicationUserRoles => Set<ApplicationUserRole>();
    public virtual DbSet<JwtToken> JwtTokens => Set<JwtToken>();
}
