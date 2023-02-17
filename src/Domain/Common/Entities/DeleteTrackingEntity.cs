namespace Domain.Common.Entities;

public abstract class DeleteTrackingEntity<TKey> : AuditableEntity<TKey>, Zord.Core.Domain.Interfaces.IDeleteTracking
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
}

public abstract class DeleteTrackingEntity : DeleteTrackingEntity<string>
{
}