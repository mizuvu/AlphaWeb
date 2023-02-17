namespace Domain.Common.Entities;

public abstract class EntityBase<TId> : EventEntity, Zord.Core.Domain.Interfaces.IEntity<TId>
{
    public virtual TId Id { get; protected set; } = default!;
}

public abstract class EntityBase : EntityBase<string>
{
    protected EntityBase() => Id = DataProvider.NewId();
}