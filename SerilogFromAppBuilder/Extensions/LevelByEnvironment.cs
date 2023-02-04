namespace SerilogFromAppBuilder.Extensions;

public static class LevelByEnvironment
{
    public static LoggerConfiguration SetLevelByEnvironment(
        this LoggerConfiguration loggerConfiguration,
        LevelEnvironment levelEnvironment = LevelEnvironment.Production)
    {
        return levelEnvironment switch
        {
            LevelEnvironment.Development => loggerConfiguration
                                            // Set Minimum LogLevel
                                            .MinimumLevel.Is(LogEventLevel.Information)
                                            // Override Minimum LogLevel for a Namespace or TypeName
                                            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning),
            LevelEnvironment.Staging => loggerConfiguration.MinimumLevel.Is(LogEventLevel.Information),
            LevelEnvironment.Production => loggerConfiguration.MinimumLevel.Is(LogEventLevel.Warning),
            _ => throw new ArgumentException("levelEnvironment not valid", nameof(levelEnvironment), null)
        };
    }
}