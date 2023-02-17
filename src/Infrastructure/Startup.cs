using Application.Common.Interfaces;
using Infrastructure.Auth;
using Infrastructure.Auth.Jwt;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuth(config);

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config, bool useJwtAuth = false)
    {
        services.AddIdentityData<AppIdentityDbContext>(config);

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddPermissions();
        services.AddIdentitySetup<AppIdentityDbContext>();

        // Must add identity before adding auth!
        services.ConfigureJwtSettings(config);
        services.AddJwtAuth(config); // inject this for use jwt auth

        services.AddClaimStores<CustomAuthDataProvider>();
        services.AddIdentityServices();

#pragma warning disable CA1416
        services.AddActiveDirectoryServices(config);
#pragma warning restore CA1416

        return services;
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder, IConfiguration config)
    {
        if (config.GetValue<bool>("AllowAnonymous"))
        {
            builder.MapControllers().AllowAnonymous();
        }
        else
        {
            builder.MapControllers().RequireAuthorization();
        }

        return builder;
    }
}
