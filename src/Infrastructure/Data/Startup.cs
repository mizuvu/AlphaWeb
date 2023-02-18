using Application.Common.Interfaces;
using Infrastructure.Data.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class Startup
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("MemoryDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(defaultConnection));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}