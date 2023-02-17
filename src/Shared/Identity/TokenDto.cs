namespace Shared.Identity;

public class TokenDto
{
    public string AccessToken { get; set; } = null!;
    public int TokenExpiryTime { get; set; }
    public string RefreshToken { get; set; } = null!;
    public int RefreshTokenExpiryTime { get; set; }
}
