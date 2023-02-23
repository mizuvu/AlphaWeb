using Application.Contracts.Dtos.Notifications;

namespace Application.Features.Notifications.Queries;

public record GetNotificationById(string Id) : IQuery<NotificationDto>;

internal class GetNotificationByIdHandler
    : IQueryHandler<GetNotificationById, NotificationDto>
{
    private readonly IApplicationDbContext _context;

    public GetNotificationByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<NotificationDto> Handle(GetNotificationById request, CancellationToken cancellationToken)
    {
        return await _context.Set<Notification>()
            .Where(x => x.Id == request.Id)
            .AsNoTracking()
            .ProjectToType<NotificationDto>()
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Data not found.");
    }
}
