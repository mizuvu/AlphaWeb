using Infrastructure;
using Serilog;
using WebApi.Extensions;
using Zord.Extensions.Logging;

Serilogger.EnsureInitialized();
Log.Information("Starting up...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog.
    builder.Host.UseSerilog(Serilogger.Configure);

    // Add services to the container.

    // Add seeder
    //builder.Services.AddSeeder(builder.Configuration);

    builder.Services.ConfigureServices(builder.Configuration);

    /*
    builder.Services
        .AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters();
    */
    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    // seed data
    //await app.Services.InitializeDatabasesAsync();

    app.ConfigurePipelines(builder.Configuration);

    app.MapEndpoints(builder.Configuration);

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Serilogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete.");
    Log.CloseAndFlush();
}
