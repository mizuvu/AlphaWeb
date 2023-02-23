using Infrastructure;
using Serilog;
using Web.Extensions;
using Zord.Extensions.Logging;

Serilogger.EnsureInitialized();
Log.Information("Starting up...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog.
    builder.Host.UseSerilog(Serilogger.Configure);

    // Add services to the container.

    builder.Services.ConfigureServices(builder.Configuration);

    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.ConfigurePipelines(builder.Configuration);

    app.MapEndpoints(builder.Configuration);

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

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
