using Microsoft.AspNetCore.Identity;
using Zord.Core.Domain.Interfaces;

namespace Infrastructure.Identity.Models;

public class ApplicationRole : IdentityRole, IAuditableEntity
{
    public string? Description { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}