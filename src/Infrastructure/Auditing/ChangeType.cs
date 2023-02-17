namespace Infrastructure.Auditing;

public enum ChangeType : byte
{
    None = 0,
    Create = 1,
    Update = 2,
    Delete = 3
}