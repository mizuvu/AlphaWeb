using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migrator.Extensions;
using Migrator.MSSQL;

// set Environment
//Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Live");

using var host = Host.CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

var initialiser = serviceProvider.GetRequiredService<AppDbContextInitialiser>();
await initialiser.InitialiseAsync();
await initialiser.SeedAsync();

var _logger = serviceProvider.GetRequiredService<ILogger<AppDbContextInitialiser>>();
_logger.LogInformation("Done.");
Console.ReadKey();
