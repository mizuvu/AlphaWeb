using Serilog;
using Serilog.Core;
using System.Text.Json;

namespace Infrastructure.Middlewares;

public static class MiddlewareLogger
{
    // https://github.com/serilog/serilog-sinks-file#rolling-policies

    private static Logger? _requestLogger;

    public static void RequestLogger(string type, string key, string logContent, object data)
    {
        _requestLogger ??= new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(@"logs\request-.txt",
                    rollingInterval: RollingInterval.Day, retainedFileCountLimit: null,
                    shared: true)
                .CreateLogger();

        var jsonData = data is null ? string.Empty : data is string ? data : JsonSerializer.Serialize(data);

        var content = $"\t{type}\t{key}\t{logContent}\t{jsonData}";

        _requestLogger.Information(content);
    }
}
