using System.Text.Json;

namespace HttpApi.Client.Common;

public static class ResultExtensions
{
    public static async Task<T> ReadObjAsync<T>(this HttpResponseMessage message)
    {
        var responseString = await message.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var obj = JsonSerializer.Deserialize<T>(responseString, options);

        ArgumentNullException.ThrowIfNull(obj);

        return obj;
    }

    public static async Task<Result> ReadObjAsync(this HttpResponseMessage message)
    {
        if (message.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        var resultStr = await message.Content.ReadAsStringAsync();

        return Result.Failure(resultStr);
    }
}
