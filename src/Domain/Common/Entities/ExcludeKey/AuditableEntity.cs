namespace Domain.Common.Entities.ExcludeKey;

public abstract class AuditableEntity : EntityBase, Zord.Core.Domain.Interfaces.IAuditableEntity
{
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}