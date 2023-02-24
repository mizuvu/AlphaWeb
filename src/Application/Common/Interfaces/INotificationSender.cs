using Application.Contracts.Notifications;

namespace Application.Common.Interfaces;

public interface INotificationSender
{
    /// <summary>
    /// Notify user has new notification
    /// </summary>
    Task NotifyToAllAsync(CancellationToken cancellationToken = default);
    Task NotifyToUserAsync(string userId, CancellationToken cancellationToken = default);
    Task NotifyToUsersAsync(string[] userIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send message to users
    /// </summary>
    Task SendToAllAsync(INotificationMessage notification,
        CancellationToken cancellationToken = default);
    Task SendToUserAsync(INotificationMessage notification, string userId,
        CancellationToken cancellationToken = default);
    Task SendToUsersAsync(INotificationMessage notification, string[] userIds,
        CancellationToken cancellationToken = default);
}