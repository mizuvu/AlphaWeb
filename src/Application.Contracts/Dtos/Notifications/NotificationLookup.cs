namespace Application.Contracts.Dtos.Notifications;

public class NotificationLookup : PageLookup
{
    public string ToUser { get; set; } = null!;
}
