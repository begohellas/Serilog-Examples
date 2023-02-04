namespace SerilogFromAppBuilder.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var endpoint = httpContext.GetEndpoint();
            _logger.LogError(ex.Demystify(), "error from endpoint {EndpointNameData}", endpoint?.DisplayName);

            var apiError = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Global Exception Middleware",
                Detail = ex.Message
            };
            apiError.Extensions.Add("EndpointNameData", endpoint?.DisplayName);

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(apiError));
        }
    }
}