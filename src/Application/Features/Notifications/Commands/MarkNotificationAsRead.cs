﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Commands;

public record MarkNotificationAsRead(string Id) : INonQuery;

internal class MarkNotificationAsReadHandler
    : INonQueryHandler<MarkNotificationAsRead>
{
    private readonly IApplicationDbContext _context;

    public MarkNotificationAsReadHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(MarkNotificationAsRead request, CancellationToken cancellationToken)
    {
        var entry = await _context.Set<Notification>().SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Notification entry {request.Id} not found");

        entry.MarkAsRead = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}