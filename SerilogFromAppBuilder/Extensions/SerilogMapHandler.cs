using SerilogFromAppBuilder.ImprovedLog;
using SerilogFromAppBuilder.Models;

namespace SerilogFromAppBuilder.Extensions;

public class SerilogMapHandler
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/base", ExampleLoggingBase)
            .WithName("base");

        app.MapGet("/withscopes", ExampleLoggingWithScopes)
            .WithName("withScopes");

        app.MapGet("/expandArgument", ExampleLoggingWithExpandArguments)
            .WithName("expandArgument");

        app.MapGet("/error", ExampleLoggingExceptionByGlobalMiddleware)
            .WithName("logexception");
    }

    private static IResult ExampleLoggingBase(ILogger<Program> logger, HttpRequest request)
    {
        // LogDebug not printed with env development by actual configuration
        // (see LevelByEnvironment.SetLevelByEnvironment)
        logger.LogDebug("Not printed in sinks with actual configuration");

        // LogInformation not printed with env production by actual configuration
        logger.LogInformation("Printed in sinks with actual configuration");

        logger.LogError($"xxx NOT use string interpolation on logs with params object... {request.Path} xxx");
        logger.LogWarning("!!! USE message template on logs with params object... {Path} !!!", request.Path);

        logger.LogWithSourceGenerator(request.Path);

        return Results.Ok("Hello GitHub!");
    }

    private static IResult ExampleLoggingWithScopes(ILogger<Program> logger, HttpRequest request)
    {
        var dicScopes = new Dictionary<string, object>
        {
            {"Scheme", request.Scheme},
            {"Host", request.Host},
            {"Path", request.Path},
            {"Method", request.Method},
        };

        // use scope to add custom properties to all log in a given execution context
        using (logger.BeginScope(dicScopes))
        {
            logger.LogInformation("Hello");
            logger.LogInformation("GitHub");
        }

        return Results.Ok("Hello GitHub!");
    }

    private static IResult ExampleLoggingWithExpandArguments(ILogger<Program> logger)
    {
        var segment = new Segment(new Coordinate(1, 2), new Coordinate(4, 6));
        var distance = segment.GetDistanceTo();

        // the @ symbol means that Serilog must expand the argument's structure as an object
        logger.LogInformation("The distance of coordinate {@Segment} is {Distance}", segment, distance);

        return Results.Ok(new
        {
            segment,
            distance
        });
    }

    private static void ExampleLoggingExceptionByGlobalMiddleware()
    {
        throw new InvalidOperationException("test Serilog logError");
    }
}