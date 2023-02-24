using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cors;

public static class Startup
{
    private const string _corsPolicyName = "CorsPolices";

    private static bool EnableCors(this IConfiguration configuration) =>
        configuration.GetValue<bool>("CORS:Enable");

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.EnableCors())
        {
            bool allowAll = configuration.GetValue<bool>("CORS:AllowAll");

            if (allowAll)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(_corsPolicyName, builder =>
                        builder.AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader());
                });

                Console.WriteLine("----- CORS allow all");
            }
            else
            {
                var origins = new List<string>();

                string? apiGateways = configuration.GetValue<string>("CORS:ApiGw");
                if (!string.IsNullOrEmpty(apiGateways))
                    origins.AddRange(apiGateways.Split(';', StringSplitOptions.RemoveEmptyEntries));

                string? blazor = configuration.GetValue<string>("CORS:Blazor");
                if (!string.IsNullOrEmpty(blazor))
                    origins.AddRange(blazor.Split(';', StringSplitOptions.RemoveEmptyEntries));

                string? mvc = configuration.GetValue<string>("CORS:Mvc");
                if (!string.IsNullOrEmpty(mvc))
                    origins.AddRange(mvc.Split(';', StringSplitOptions.RemoveEmptyEntries));

                if (origins.Any())
                {
                    services.AddCors(opt =>
                        opt.AddPolicy(_corsPolicyName, policy =>
                            policy.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins(origins.ToArray())));
                }

                Console.WriteLine("----- CORS enabled");
            }
        }

        return services;
    }

    public static IApplicationBuilder UseCorsPolicies(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (configuration.EnableCors())
        {
            app.UseCors(_corsPolicyName);
        }

        return app;
    }
}