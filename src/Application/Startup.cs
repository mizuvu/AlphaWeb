using Application.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var domainAssembly = AppDomain.CurrentDomain.GetAssemblies();
        var sharedAssembly = typeof(Shared.Startup).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.AddValidatorsFromAssembly(sharedAssembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(domainAssembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}