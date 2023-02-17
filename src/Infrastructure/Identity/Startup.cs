using Application.Identity;
using Infrastructure.Identity.Abstractions;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Versioning;

namespace Infrastructure.Identity;

public static class Startup
{
    public static IServiceCollection AddIdentityData<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : IdentityDbContext
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services
                .AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase("MemoryDb"));
        }
        else
        {
            services.AddDbContext<TContext>(opt =>
                opt.UseSqlServer(defaultConnection,
                    m => m.MigrationsAssembly("Migrator")));
        }

        services.AddScoped<IIdentityDbContext>(provider => provider.GetRequiredService<TContext>());

        return services;
    }

    public static IServiceCollection AddIdentitySetup<TContext>(this IServiceCollection services)
        where TContext : IdentityDbContext
    {
        services
            //.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            .AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;

                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
                //options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<TContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddClaimStores<TCustomAuthDataProvider>(this IServiceCollection services)
        where TCustomAuthDataProvider : class, IAuthDataProvider
    {
        services.AddScoped<IAuthDataProvider, TCustomAuthDataProvider>();

        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    [SupportedOSPlatform("windows")]
    public static IServiceCollection AddActiveDirectoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        var activeDirectoryConfig = configuration.GetSection("ActiveDirectory");

        services.Configure<ActiveDirectory>(activeDirectoryConfig);
        var settings = activeDirectoryConfig.Get<ActiveDirectory>();

        ArgumentNullException.ThrowIfNull(settings, "ActiveDirectory");

        switch (settings.PreferLDAP)
        {
            case true:
                services.AddScoped<IActiveDirectoryService, LDAPService>();
                break;
            default:
                services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
                break;
        }

        return services;
    }

}