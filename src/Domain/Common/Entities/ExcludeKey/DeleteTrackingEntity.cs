namespace Domain.Common.Entities.ExcludeKey;

public abstract class DeleteTrackingEntity : AuditableEntity, Zord.Core.Domain.Interfaces.IDeleteTracking
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
}