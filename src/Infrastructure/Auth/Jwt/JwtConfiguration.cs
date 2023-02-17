namespace Infrastructure.Auth.Jwt;

public class JwtConfiguration
{
    public string TokenIssuer { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public int TokenExpireInSeconds { get; set; }
    public int RefreshTokenExpireInSeconds { get; set; }
}