using Application.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SignalR;

public static class Startup
{
    private static bool EnableSignalR(this IConfiguration configuration) =>
        configuration.GetValue<bool>("SignalR:Enable");

    public static IServiceCollection AddNotification(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.EnableSignalR())
        {
            services.AddSignalR();

            /* use only for Services API */
            //services.AddSingleton<IUserIdProvider, CustomIdProvider>();

            services.AddScoped<NotificationHub>();

            services.AddScoped<INotificationSender, NotificationSender>();

            Console.WriteLine("----- SignalR enabled");
        }

        return services;
    }

    public static IEndpointRouteBuilder MapNotificationHub(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
    {
        if (configuration.EnableSignalR())
        {
            var hub = configuration.GetValue<string>("SignalR:HubEndpoint");

            endpoints.MapHub<NotificationHub>(hub ?? "/hub", options =>
            {
                options.CloseOnAuthenticationExpiration = true;
            });
        }

        return endpoints;
    }
}