using WebClient.Infrastructure.Auth.Permissions;

namespace WebClient.Infrastructure.Auth;

internal static class Startup
{
    internal static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services.AddSingleton<ICurrentUser, CurrentUser>();

    internal static IServiceCollection AddPermissions(this IServiceCollection services) =>
        services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
}
/*
services
    .AddTransient<ExceptionHandlerMiddleware>()
    .AddScoped<MvcTokenProvider>()
    .AddScoped<JwtAuthenticationHeaderHandler>()
    .AddApiClient(configuration)
    .AddHttpClient(IdentityApi, client =>
    {
        client.BaseAddress = new Uri(configuration["IdentityApiHost"]);
    })
    .AddHttpMessageHandler<JwtAuthenticationHeaderHandler>().Services
    .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(IdentityApi));
*/