using HttpApi.Client.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebClient.Infrastructure.Auth;

namespace WebClient.Extensions;

public static class ServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddCurrentUser();
        services.AddPermissions();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
           {
               // Cookie settings
               //options.Cookie.HttpOnly = true;
               options.ExpireTimeSpan = TimeSpan.FromHours(12);

               options.LoginPath = "/Account/Login";
               options.AccessDeniedPath = "/Account/AccessDenied";
               options.SlidingExpiration = true;
           });

        services.AddScoped<MvcTokenProvider>();
        services.AddScoped<JwtAuthenticationHeaderHandler>();

        services.AddApiClients();

        var apiClients = typeof(HttpApiClientConsts).GetPropertyNames();
        foreach (var clientName in apiClients)
        {
            services
                .AddHttpClient(clientName, client =>
                {
                    client.BaseAddress = new Uri(configuration[clientName]!);
                })
                .AddHttpMessageHandler<JwtAuthenticationHeaderHandler>().Services
                .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(clientName));
        }
    }
}