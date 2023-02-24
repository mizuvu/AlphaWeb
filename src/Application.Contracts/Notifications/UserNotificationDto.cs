namespace Application.Contracts.Notifications;

public class UserNotificationDto
{
    public string UserId { get; set; } = null!;
    public int UnRead { get; set; }
    public PagedList<NotificationDto> Entries { get; set; } = default!;
}
