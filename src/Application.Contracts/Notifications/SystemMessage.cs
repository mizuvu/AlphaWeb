﻿namespace Application.Contracts.Notifications;

public class NotifyMessage : INotificationMessage
{
    public string FromUserId { get; set; } = null!;
    public string? FromName { get; set; }
    public string ToUserId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Message { get; set; }
    public string? Url { get; set; }
}