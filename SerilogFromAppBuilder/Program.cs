using SerilogFromAppBuilder.Extensions;
using SerilogFromAppBuilder.Middlewares;
using SerilogFromAppBuilder.SerilogEnrichers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Serilog Examples", Version = "v1" });
});
builder.AddSerilogProvider(builder.Configuration, builder.Environment);

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Serilog Examples");
});

/*
 * Reducing log verbosity
 * https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity/
 */
app.UseSerilogRequestLogging(options => options.EnrichDiagnosticContext = EnrichContextHelper.EnrichContext);

/* Serilog Example */
var serilogHandler = new SerilogMapHandler();
serilogHandler.MapEndpoints(app);

app.Run();