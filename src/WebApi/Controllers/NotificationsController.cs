using Application.Common.Interfaces;
using Application.Contracts.Authorization;
using Application.Contracts.Identity;
using Application.Features.Notifications.Commands;
using Application.Features.Notifications.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class NotificationsController : VersionedApiControllerBase
{
    private readonly ICurrentUser _currentUser;
    private readonly INotificationSender _notificationSender;

    public NotificationsController(ICurrentUser currentUser,
        INotificationSender notificationSender)
    {
        _currentUser = currentUser;
        _notificationSender = notificationSender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(GetUserNotifications request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var data = await Mediator.Send(new GetNotificationById(id), cancellationToken);

        if (_currentUser.UserId == data.ToUserId)
            await Mediator.Send(new MarkNotificationAsRead(id), cancellationToken);

        return Ok(data);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Managers.Push)]
    public async Task<IActionResult> PostAsync([FromBody] AddNotification request)
    {
        var response = await Mediator.Send(request);

        await _notificationSender.NotifyToUserAsync(request.ToUserId);

        return Ok(response);
    }
}