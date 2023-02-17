using MediatR;

namespace Domain.Common.Entities;

public abstract class DomainEvent : INotification
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}