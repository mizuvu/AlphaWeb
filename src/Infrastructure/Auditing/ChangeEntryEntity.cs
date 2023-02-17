using MassTransit;

namespace Infrastructure.Auditing;

public class ChangeEntryEntity
{
    public ChangeEntryEntity() => Id = NewId.Next().ToString();

    public string Id { get; set; } = null!;
    public string? UserId { get; set; }
    public string? Type { get; set; }
    public string? TableName { get; set; }
    public DateTimeOffset ChangeOnTime { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? AffectedColumns { get; set; }
    public string? PrimaryKey { get; set; }
}