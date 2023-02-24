using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Swagger;

namespace WebApi.Swagger;

public static class Startup
{
    private static bool EnableSwagger(this IConfiguration configuration) =>
        configuration.GetValue<bool>("Swagger:Enable");

    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.EnableSwagger())
        {
            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            Console.WriteLine("----- Swagger enabled");
        }

        return services;
    }

    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (configuration.EnableSwagger())
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }

        return app;
    }
}