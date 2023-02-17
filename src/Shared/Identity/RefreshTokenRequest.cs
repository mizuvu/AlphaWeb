namespace Shared.Identity;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);