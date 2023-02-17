using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middlewares;

public static class Startup
{
    public static IServiceCollection AddMiddleware(this IServiceCollection services, IConfiguration config)
    {
        if (config.GetValue<bool>("ExceptionMiddleware"))
        {
            services.AddScoped<ExceptionMiddleware>();
        }

        if (config.GetValue<bool>("RequestLogMiddleware"))
        {
            services.AddScoped<RequestLogMiddleware>();
        }

        return services;
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app, IConfiguration config)
    {
        if (config.GetValue<bool>("ExceptionMiddleware"))
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        return app;
    }

    public static IApplicationBuilder UseSaveRequestLogMiddleware(this IApplicationBuilder app, IConfiguration config)
    {
        if (config.GetValue<bool>("RequestLogMiddleware"))
        {
            app.UseMiddleware<RequestLogMiddleware>();
        }

        return app;
    }
}