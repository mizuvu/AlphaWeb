using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Auth.Jwt;

public static class Startup
{
    public static void ConfigureJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtAuth"));
    }

    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("JwtAuth").Get<JwtConfiguration>();

        ArgumentNullException.ThrowIfNull(jwtConfig);

        var secretKey = Encoding.ASCII.GetBytes(jwtConfig.SecretKey);
        var issuer = jwtConfig.TokenIssuer;

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                bearer.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            throw new UnauthorizedException("Authentication Failed.");
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = _ => throw new ForbiddenException("You are not authorized to access this resource."),
                    OnMessageReceived = context =>
                    {
                        if (context.HttpContext.Request.Path.StartsWithSegments("/push"))
                        {
                            var accessToken = context.Request.Query["access_token"];

                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}