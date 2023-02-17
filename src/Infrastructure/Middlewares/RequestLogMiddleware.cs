using Microsoft.AspNetCore.Http;
using System.Text;

namespace Infrastructure.Middlewares;

public class RequestLogMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method != HttpMethod.Get.Method)
        {
            var remoteIp = context.Connection.RemoteIpAddress;

            //_logger.LogWarning("Request from Remote IP address: {RemoteIp}", remoteIp);

            string path = context.Request.Path;
            string requestBody = string.Empty;

            var request = context.Request;

            if (!string.IsNullOrEmpty(request.ContentType)
                && request.ContentType.StartsWith("application/json"))
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 4096, true);
                requestBody = await reader.ReadToEndAsync();

                // rewind for next middleware.
                request.Body.Position = 0;
            }

            //var requestText = $"{remoteIp} - {path}\r\n{requestBody}";
            //var name = $"[{path.Replace("/", "-")}]_{Guid.NewGuid()}";

            MiddlewareLogger.RequestLogger($"{remoteIp}", $"{path}", requestBody, string.Empty);
        }

        await next.Invoke(context);
    }
}
