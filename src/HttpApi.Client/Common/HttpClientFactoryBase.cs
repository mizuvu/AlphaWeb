using System.Net.Http.Json;

namespace HttpApi.Client.Common;

public abstract class HttpClientFactoryBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _defaultClientName = HttpApiClientConsts.DefaultClientName;

    public HttpClientFactoryBase(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected async Task<T> GetAsync<T>(string url, string? clientName = null, CancellationToken cancellationToken = default)
    {
        clientName ??= _defaultClientName;
        using var client = _httpClientFactory.CreateClient(clientName);

        var response = await client.GetAsync(url, cancellationToken);

        return await response.ReadObjAsync<T>();
    }

    protected async Task<T> PostAsJsonAsync<T>(string url, object data, string? clientName = null, CancellationToken cancellationToken = default)
    {
        clientName ??= _defaultClientName;
        using var client = _httpClientFactory.CreateClient(clientName);

        var response = await client.PostAsJsonAsync(url, data, cancellationToken);

        return await response.ReadObjAsync<T>();
    }

    protected async Task<Result> PostAsJsonAsync(string url, object data, string? clientName = null, CancellationToken cancellationToken = default)
    {
        clientName ??= _defaultClientName;
        using var client = _httpClientFactory.CreateClient(clientName);

        var response = await client.PostAsJsonAsync(url, data, cancellationToken);

        return await response.ReadObjAsync();
    }

    protected async Task<T> PutAsJsonAsync<T>(string url, object data, string? clientName = null, CancellationToken cancellationToken = default)
    {
        clientName ??= _defaultClientName;
        using var client = _httpClientFactory.CreateClient(clientName);

        var response = await client.PutAsJsonAsync(url, data, cancellationToken);

        return await response.ReadObjAsync<T>();
    }

    protected async Task<Result> PutAsJsonAsync(string url, object data, string? clientName = null, CancellationToken cancellationToken = default)
    {
        clientName ??= _defaultClientName;
        using var client = _httpClientFactory.CreateClient(clientName);

        var response = await client.PutAsJsonAsync(url, data, cancellationToken);

        return await response.ReadObjAsync();
    }

    protected async Task<T> DeleteAsync<T>(string url, string? clientName = null, CancellationToken cancellationToken = default)
    {
        clientName ??= _defaultClientName;
        using var client = _httpClientFactory.CreateClient(clientName);

        var response = await client.DeleteAsync(url, cancellationToken);

        return await response.ReadObjAsync<T>();
    }
}
