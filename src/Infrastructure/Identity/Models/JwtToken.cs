namespace Infrastructure.Identity.Models;

public class JwtToken
{
    public string UserId { get; set; } = null!;
    public string? Token { get; set; }
    public DateTimeOffset? TokenExpiryTime { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
}
