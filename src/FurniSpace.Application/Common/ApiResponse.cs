namespace FurniSpace.Application.Common;

public sealed record ApiError(
    string Code,
    string Message,
    IReadOnlyDictionary<string, string[]>? ValidationErrors,
    string TraceId);

public sealed record ApiResponse<T>(bool Success, T? Data, ApiError? Error)
{
    public static ApiResponse<T> FromSuccess(T data) => new(true, data, null);

    public static ApiResponse<T> FromFailure(
        string code,
        string message,
        string traceId,
        IReadOnlyDictionary<string, string[]>? validationErrors = null) =>
        new(false, default, new ApiError(code, message, validationErrors, traceId));
}
