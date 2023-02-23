using Application;
using Infrastructure;
using Infrastructure.Cors;
using Infrastructure.Identity.Models;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Web.Services;

namespace Web.Extensions;

public static class ServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructure(config);
        services.AddApplication();

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(12);

            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        //custom Claims for Identity user
        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppUserClaimsPrincipalFactory>();
    }

    public static IApplicationBuilder ConfigurePipelines(this IApplicationBuilder builder, IConfiguration config) =>
    builder
        .UseMiddlewares(config)
        .UseRouting()
        .UseCorsPolicies(config)
        .UseAuthentication()
        .UseAuthorization()
        .UseSerilogRequestLogging();