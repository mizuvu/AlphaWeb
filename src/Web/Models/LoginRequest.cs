using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class LoginRequest
{
    [Required]
    public string UserName { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
}
