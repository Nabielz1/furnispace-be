using FurniSpace.Application.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace FurniSpace.Api.ExceptionHandlers;

public sealed partial class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        LogUnhandledException(logger, exception, httpContext.TraceIdentifier);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            ApiResponse<object>.FromFailure(
                "UNHANDLED_ERROR",
                "An unexpected error occurred.",
                httpContext.TraceIdentifier),
            cancellationToken);

        return true;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Unhandled exception. TraceId={TraceId}")]
    private static partial void LogUnhandledException(
        ILogger logger,
        Exception exception,
        string traceId);
}
