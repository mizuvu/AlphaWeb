namespace Domain.Common.Entities;

public abstract class AuditableEntity<TKey> : EntityBase<TKey>, Zord.Core.Domain.Interfaces.IAuditableEntity
{
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}

public abstract class AuditableEntity : AuditableEntity<string>
{
    protected AuditableEntity() => Id = DataProvider.NewId();
}