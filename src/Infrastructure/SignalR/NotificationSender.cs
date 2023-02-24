using Application.Common.Interfaces;
using Application.Contracts.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.SignalR;

public class NotificationSender : INotificationSender
{
    private readonly IHubContext<NotificationHub> _notificationHubContext;
    private readonly ILogger<NotificationSender> _logger;

    public NotificationSender(
        IHubContext<NotificationHub> notificationHubContext,
        ILogger<NotificationSender> logger) =>
        (_notificationHubContext, _logger) = (notificationHubContext, logger);

    public Task NotifyToAllAsync(CancellationToken cancellationToken = default) =>
        _notificationHubContext.Clients.All
            .SendAsync(NotificationConts.NotificationFromServer, cancellationToken);

    public Task NotifyToUserAsync(string userId, CancellationToken cancellationToken = default) =>
        _notificationHubContext.Clients.User(userId)
            .SendAsync(NotificationConts.NotificationFromServer, cancellationToken);

    public Task NotifyToUsersAsync(string[] userIds, CancellationToken cancellationToken = default) =>
        _notificationHubContext.Clients.Users(userIds)
            .SendAsync(NotificationConts.NotificationFromServer, cancellationToken);

    public async Task SendToAllAsync(INotificationMessage notification,
        CancellationToken cancellationToken = default)
    {
        await _notificationHubContext.Clients.All
            .SendAsync(
                NotificationConts.MessageFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken);

        _logger.LogInformation("----- Send {message} to all users", notification.GetType().FullName);
    }

    public async Task SendToUserAsync(INotificationMessage notification, string userId,
        CancellationToken cancellationToken = default)
    {
        await _notificationHubContext.Clients.User(userId)
            .SendAsync(
                NotificationConts.MessageFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken);

        _logger.LogInformation("----- Send {message} to user {userId}",
            notification.GetType().FullName, userId);
    }

    public async Task SendToUsersAsync(INotificationMessage notification, string[] userIds,
        CancellationToken cancellationToken = default)
    {
        await _notificationHubContext.Clients.Users(userIds)
            .SendAsync(
                NotificationConts.MessageFromServer,
                notification.GetType().FullName,
                notification,
                cancellationToken);

        _logger.LogInformation("----- Send {message} to users {userIds}",
            notification.GetType().FullName, string.Join(",", userIds));
    }
}