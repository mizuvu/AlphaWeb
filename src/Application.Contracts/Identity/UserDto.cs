using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.Identity;

public class UserDto
{
    public string Id { get; set; } = default!;

    [Required]
    [MinLength(3)]
    public string UserName { get; set; } = default!;

    [Required]
    [MinLength(3)]
    public string FirstName { get; set; } = default!;

    [Required]
    [MinLength(3)]
    public string LastName { get; set; } = default!;

    [Required]
    [MinLength(3)]
    public string Password { get; set; } = default!;

    public string? Email { get; set; } = default!;

    public string? PhoneNumber { get; set; } = default!;

    public string FullName => FirstName + " " + LastName;

    public bool UseDomainPassword { get; set; }

    public bool IsBlocked { get; set; }

    public bool IsDeleted { get; set; }

    public IEnumerable<string> Roles { get; set; } = new List<string>();
}