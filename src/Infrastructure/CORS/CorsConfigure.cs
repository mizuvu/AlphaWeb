using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HO.Infrastructure.CORS;

public static class CorsConfigure
{
    private const string _corsPolicy = "ZordCorsPolices";

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration config)
    {
        bool enable = config.GetValue<bool>("CORS:Enable");

        if (enable)
        {
            bool allowAll = config.GetValue<bool>("CORS:AllowAll");

            if (allowAll)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(_corsPolicy, builder =>
                        builder.AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader());
                });

                Console.WriteLine("----- Cors allow all");
            }
            else
            {
                var origins = new List<string>();

                string? apiGateways = config.GetValue<string>("CORS:ApiGw");
                if (!string.IsNullOrEmpty(apiGateways))
                    origins.AddRange(apiGateways.Split(';', StringSplitOptions.RemoveEmptyEntries));

                string? blazor = config.GetValue<string>("CORS:Blazor");
                if (!string.IsNullOrEmpty(blazor))
                    origins.AddRange(blazor.Split(';', StringSplitOptions.RemoveEmptyEntries));

                string? mvc = config.GetValue<string>("CORS:Mvc");
                if (!string.IsNullOrEmpty(mvc))
                    origins.AddRange(mvc.Split(';', StringSplitOptions.RemoveEmptyEntries));

                if (origins.Any())
                {
                    services.AddCors(opt =>
                        opt.AddPolicy(_corsPolicy, policy =>
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

    public static IApplicationBuilder UseCorsPolicies(this IApplicationBuilder app, IConfiguration config)
    {
        bool enable = config.GetValue<bool>("CorsSettings:Enable");

        if (enable)
        {
            app.UseCors(_corsPolicy);
        }

        return app;
    }
}