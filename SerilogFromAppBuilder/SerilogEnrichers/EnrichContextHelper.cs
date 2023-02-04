namespace SerilogFromAppBuilder.SerilogEnrichers;

public static class EnrichContextHelper
{
    public static void EnrichContext(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        var request = httpContext.Request;

        diagnosticContext.Set("Host", request.Host.ToString());
        diagnosticContext.Set("Protocol", request.Protocol);
        diagnosticContext.Set("Scheme", request.Scheme);
        diagnosticContext.Set("IpAddress", httpContext.Connection.RemoteIpAddress?.ToString());

        if (request.QueryString.HasValue)
        {
            diagnosticContext.Set("QueryString", request.QueryString.Value);
        }

        // Set the content-type of the ResponseApi at this point
        diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

        // Retrieve the IEndpointFeature selected for the request
        var endpoint = httpContext.GetEndpoint();
        if (endpoint is { })
        {
            diagnosticContext.Set("EndpointName", endpoint.DisplayName);
        }
    }
}