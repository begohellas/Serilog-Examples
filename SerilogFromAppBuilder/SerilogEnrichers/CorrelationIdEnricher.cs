namespace SerilogFromAppBuilder.SerilogEnrichers;

public class CorrelationIdEnricher: ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;
        if (activity is null)
        {
            return;
        }

        logEvent.AddPropertyIfAbsent(new LogEventProperty("CorrelationId", new ScalarValue(activity.RootId ?? Guid.NewGuid().ToString())));
    }
}