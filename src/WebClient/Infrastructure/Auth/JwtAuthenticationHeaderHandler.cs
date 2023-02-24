using System.Net.Http.Headers;

namespace WebClient.Infrastructure.Auth;

public class JwtAuthenticationHeaderHandler : DelegatingHandler
{
    private readonly MvcTokenProvider _tokenProvider;

    public JwtAuthenticationHeaderHandler(MvcTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // skip token endpoints
        if (request.RequestUri?.AbsolutePath.Contains("/token") is not true)
        {
            var token = _tokenProvider.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}