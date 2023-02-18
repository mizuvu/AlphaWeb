using Application;
using Infrastructure;
using Infrastructure.CORS;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Extensions;
using WebApi.Swagger;

namespace WebApi.Extensions;

public static class ServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructure(config);
        services.AddApplication();

        services.AddSwagger(config);
        services.AddApiVersioning(1);
    }

    public static IApplicationBuilder ConfigurePipelines(this IApplicationBuilder builder, IConfiguration config) =>
    builder
        .UseExceptionMiddleware(config)
        .UseRouting()
        .UseCorsPolicies(config)
        .UseAuthentication()
        .UseAuthorization()
        .UseSwagger(config)
        .UseSerilogRequestLogging()
        .UseSaveRequestLogMiddleware(config);

    private static IServiceCollection AddApiVersioning(this IServiceCollection services, int version) =>
    services.AddApiVersioning(config =>
    {
        config.DefaultApiVersion = new ApiVersion(version, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
        config.ReportApiVersions = true;
    });

    public static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder host)
    {
        var env = host.Environment.EnvironmentName;
        var configuration = host.Configuration;

        configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

        // load *.json file from folder Configurations
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Configurations");
        var dInfo = new DirectoryInfo(path);
        var files = dInfo.GetFiles("*.json");

        foreach (var file in files)
        {
            configuration
                .AddJsonFile(Path.Combine(path, file.Name), optional: false, reloadOnChange: true);
        }

        configuration.AddEnvironmentVariables();

        return host;
    }
}