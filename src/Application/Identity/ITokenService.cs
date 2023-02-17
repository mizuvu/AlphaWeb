using Shared.Identity;

namespace Application.Identity;

public interface ITokenService
{
    Task<TokenDto> GetTokenAsync(TokenRequest request);
    Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request);
}