// Program.cs

using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

// See https://github.com/dotnet/AspNetCore.Docs/issues/23387#issuecomment-927317675
WebApplicationOptions options = new()
{
    Args = args,
    // Sets the content root to AppContext.BaseDirectory.
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};

WebApplicationBuilder builder = WebApplication.CreateBuilder(options);

// Sets the host lifetime to WindowsServiceLifetime.
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = ".NET Example WebApp";
});

builder.Logging.AddEventLog(options =>
{
    options.SourceName = ".NET Example WebApp";
});

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

WebApplication app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
