using Application.Contracts.Identity;

namespace HttpApi.Client;

public class TokenClient : HttpClientFactoryBase, IApiClient
{
    private const string _path = "api/v1/oauth";

    public TokenClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<TokenDto> GetTokenAsync(TokenRequest request)
    {
        var url = $"{_path}/token";

        return await PostAsJsonAsync<TokenDto>(url, request);
    }

    public async Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var url = $"{_path}/token/refresh";

        return await PostAsJsonAsync<TokenDto>(url, request);
    }
}
