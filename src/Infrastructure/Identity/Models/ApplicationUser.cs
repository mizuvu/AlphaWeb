using Microsoft.AspNetCore.Identity;
using Zord.Core.Domain.Interfaces;

namespace Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser, IAuditableEntity, IDeleteTracking
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public bool UseDomainPassword { get; set; }
    public bool IsBlocked { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
}
