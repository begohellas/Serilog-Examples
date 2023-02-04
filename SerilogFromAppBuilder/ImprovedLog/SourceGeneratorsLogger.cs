namespace SerilogFromAppBuilder.ImprovedLog;

public static partial class SourceGeneratorsLogger
{
    /*
     * NET 6 introduces the LoggerMessageAttribute type
     * https://andrewlock.net/exploring-dotnet-6-part-8-improving-logging-performance-with-source-generators/
     */
    [LoggerMessage(
        EventId = 0,
        EventName = "LogWithSourceGenerator",
        Level = LogLevel.Information,
        Message = "With .NET 6 improved log with sourcegenerator... {path}"
    )]
    public static partial void LogWithSourceGenerator(this ILogger logger, string path);
}