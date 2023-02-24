namespace Application.Features.Notifications.Commands;

public class AddNotification : ICommand<Result>
{
    public string FromUserId { get; set; } = null!;
    public string? FromName { get; set; }
    public string ToUserId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Message { get; set; }
    public string? Url { get; set; }
}

internal class AddNotificationHandler : ICommandHandler<AddNotification, Result>
{
    private readonly IApplicationDbContext _context;

    public AddNotificationHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(AddNotification request, CancellationToken cancellationToken)
    {
        var entity = new Notification
        {
            FromUserId = request.FromUserId,
            FromName = request.FromName,
            ToUserId = request.ToUserId,
            Title = request.Title,
            Message = request.Message,
            Url = request.Url
        };

        await _context.Set<Notification>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
