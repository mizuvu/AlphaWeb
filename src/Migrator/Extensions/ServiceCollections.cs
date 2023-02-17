using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrator.MSSQL;

namespace Migrator.Extensions
{
    public static class ServiceCollections
    {
        /// <summary>
        ///     Register DI when run console.
        /// </summary>
        public static IServiceCollection AddServiceCollections(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityData<AppIdentityDbContext>(config);

            services.AddScoped<ICurrentUser, Services.CurrentUser>();

            services.AddIdentitySetup<AppIdentityDbContext>();

            // manual inject services here
            services.AddScoped<AppDbContextInitialiser>();

            return services;
        }
    }
}
