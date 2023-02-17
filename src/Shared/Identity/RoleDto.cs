namespace Shared.Identity;

public class RoleDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public IEnumerable<string> Claims { get; set; } = new List<string>();
}
