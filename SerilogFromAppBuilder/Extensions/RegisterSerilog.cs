using Serilog.Formatting.Json;
using SerilogFromAppBuilder.SerilogEnrichers;

namespace SerilogFromAppBuilder.Extensions;

public static class RegisterSerilog
{
    private const string OutputTemplateConsole =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}";

    /// <summary>
    /// output template with scopes data
    /// </summary>
    private const string OutputTemplateIncludeScope =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} Properties={Properties:j}{NewLine}{Exception}";

    /// <summary>
    /// Integrate Serilog logger into application with vary enrichment.
    /// Set minimum level by environment.
    /// </summary>
    /// <param name="builder">WebApplicationBuilder.</param>
    /// <param name="config">IConfiguration.</param>
    /// <param name="environment">IWebHostEnvironment.</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddSerilogProvider(this WebApplicationBuilder builder, IConfiguration config, IWebHostEnvironment environment)
    {
        builder.Services.AddHttpContextAccessor();

        var levelByEnvironment = SetLevelByEnvironment(environment);

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .SetLevelByEnvironment(levelByEnvironment)
            //.MinimumLevel(LogEventLevel.Information)
            .Enrich.FromLogContext()
            // Enriches logs events with information from the process environment
            // nuget Serilog.Enrichers.Environment
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            // Enriches logs with client IP and UserAgent
            // nuget Serilog.Enrichers.ClientInfo
            .Enrich.WithClientIp()
            .Enrich.WithClientAgent()
            // Enriches logs with custom property or enricher
            .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name ?? "SerilogExamples")
            .Enrich.With(new CorrelationIdEnricher(), new LocalTimestampEnricher())
            .Enrich.With<UserNameEnricher>()
            // Sinks log to Console
            .WriteTo.Console(outputTemplate: OutputTemplateConsole) //OutputTemplateIncludeScope
                                                                    // Sinks log to File
                                                                    //nuget Serilog.Sinks.File
            .WriteTo.File(formatter: new JsonFormatter(), // you can use CompactJsonFormatter to reduce space used
                        path: "../logs/webapi-.log", // automatically serilog concat date at end to name file
                        fileSizeLimitBytes: 10_485_760,
                        buffered: true,
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true)
            .WriteTo.Seq(serverUrl: "http://localhost:5341")
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(logger);

        return builder;
    }

    private static LevelEnvironment SetLevelByEnvironment(IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return LevelEnvironment.Development;
        }

        if (environment.IsStaging())
        {
            return LevelEnvironment.Staging;
        }

        return LevelEnvironment.Production;
    }
}