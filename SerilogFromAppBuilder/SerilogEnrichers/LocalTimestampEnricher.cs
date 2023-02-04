namespace SerilogFromAppBuilder.SerilogEnrichers;

public class LocalTimestampEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("LocalTimestamp", logEvent.Timestamp.ToLocalTime().ToString()));
    }
}