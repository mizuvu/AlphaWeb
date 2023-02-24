using Microsoft.Extensions.DependencyInjection;

namespace HttpApi.Client;

public static class Startup
{
    public static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        services.AutoAddServices();

        return services;
    }

    //here auto register DI implement from IApi
    private static void AutoAddServices(this IServiceCollection services)
    {
        var iService = typeof(IApiClient);

        var types = iService
            .Assembly
            .GetExportedTypes()
            .Where(t => iService.IsAssignableFrom(t) && t.Name != iService.Name) //select services implement from IService
            .Select(t => new
            {
                InterfaceService = t.GetInterfaces().FirstOrDefault(x => x != iService), //.GetInterface($"I{t.Name}")
                Service = t.Name,
                Implementation = t
            })
            .Where(t => t.Service != null);

        foreach (var type in types)
        {
            if (type.InterfaceService != null)
            {
                services.AddScoped(type.InterfaceService, type.Implementation);
            }
            else
            {
                if (!type.Implementation.IsInterface)
                {
                    services.AddScoped(type.Implementation);
                }
            }
        }
    }
}