using Application.Common.Interfaces;
using Infrastructure.Auth;
using Infrastructure.Auth.Jwt;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Services;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);

        services.AddData(configuration);

        services.AddMiddlewares(configuration);

        return services;
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("AllowAnonymous"))
        {
            builder.MapControllers().AllowAnonymous();
        }
        else
        {
            builder.MapControllers().RequireAuthorization();
        }

        return builder;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityData<AppIdentityDbContext>(configuration);

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddPermissions();
        services.AddIdentitySetup<AppIdentityDbContext>();

        // Must add identity before adding auth!
        services.ConfigureJwtSettings(configuration);
        // Inject outside because WebMVC don't use it
        //services.AddJwtAuth(config); // inject this for use jwt auth

        services.AddClaimStores<CustomAuthDataProvider>();
        services.AddIdentityServices();

#pragma warning disable CA1416
        services.AddActiveDirectoryServices(configuration);
#pragma warning restore CA1416

        return services;
    }
}
