namespace FurniSpace.Api.Middleware;

public sealed class TraceIdMiddleware(RequestDelegate next, ILogger<TraceIdMiddleware> logger)
{
    public const string HeaderName = "X-Trace-Id";

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = context.Request.Headers.TryGetValue(HeaderName, out var value)
            && !string.IsNullOrWhiteSpace(value)
                ? value.ToString()
                : Guid.NewGuid().ToString("N");

        context.TraceIdentifier = traceId;
        context.Response.Headers[HeaderName] = traceId;

        using (logger.BeginScope(new Dictionary<string, object> { ["TraceId"] = traceId }))
        {
            await next(context);
        }
    }
}
