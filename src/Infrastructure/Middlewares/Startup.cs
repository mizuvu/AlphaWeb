using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middlewares;

public static class Startup
{
    private static bool EnableExceptionMiddleware(this IConfiguration configuration) =>
        configuration.GetValue<bool>("Middlewares:ExceptionHandler");

    private static bool EnableRequestLogMiddleware(this IConfiguration configuration) =>
        configuration.GetValue<bool>("Middlewares:RequestLog");

    public static IServiceCollection AddMiddlewares(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.EnableExceptionMiddleware())
        {
            services.AddScoped<ExceptionMiddleware>();
        }

        if (configuration.EnableRequestLogMiddleware())
        {
            services.AddScoped<RequestLogMiddleware>();
        }

        return services;
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (configuration.EnableExceptionMiddleware())
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        if (configuration.EnableRequestLogMiddleware())
        {
            app.UseMiddleware<RequestLogMiddleware>();
        }

        return app;
    }
}