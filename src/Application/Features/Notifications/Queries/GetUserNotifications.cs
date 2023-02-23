using Application.Contracts.Dtos.Notifications;

namespace Application.Features.Notifications.Queries;

public class GetUserNotifications : NotificationLookup, IQuery<UserNotificationDto>
{
}

internal class GetUserNotificationsHandler
    : IQueryHandler<GetUserNotifications, UserNotificationDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserNotificationsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserNotificationDto> Handle(GetUserNotifications request, CancellationToken cancellationToken)
    {
        var query = _context.Set<Notification>().Where(x => x.ToUserId == request.ToUser).AsNoTracking();

        var unRead = await query.CountAsync(x => x.MarkAsRead == false, cancellationToken: cancellationToken);

        var entries = await query
            .OrderByDescending(o => o.CreatedOn)
            .ProjectToType<NotificationDto>()
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

        var result = new UserNotificationDto
        {
            UserId = request.ToUser,
            UnRead = unRead,
            Entries = entries,
        };

        return result;
    }
}
