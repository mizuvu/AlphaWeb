﻿using Domain.Common.Interfaces;
using MediatR;

namespace Application.Common.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IPublisher mediator, DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<IEventEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
