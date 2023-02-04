namespace SerilogFromAppBuilder.SerilogEnrichers;

public class UserNameEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserNameEnricher()
        : this(new HttpContextAccessor())
    {
    }

    public UserNameEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (!(_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false))
            return;

        // Access the name of the logged-in user
        var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
        var userNameProperty = propertyFactory.CreateProperty("UserName", userName);
        logEvent.AddPropertyIfAbsent(userNameProperty);
    }
}