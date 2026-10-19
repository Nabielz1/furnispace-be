namespace FurniSpace.Application.Common;

public sealed record ResultError(
    string Code,
    string Message,
    IReadOnlyDictionary<string, string[]>? ValidationErrors = null);

public sealed class Result<T>
{
    private Result(bool isSuccess, T? value, ResultError? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public bool IsSuccess { get; }
    public T? Value { get; }
    public ResultError? Error { get; }
    public int StatusCode { get; }

    public static Result<T> Success(T value) => new(true, value, null, 200);
    public static Result<T> Created(T value) => new(true, value, null, 201);
    public static Result<T> BadRequest(string code, string message) => new(false, default, new(code, message), 400);
    public static Result<T> Unauthorized(string code, string message) => new(false, default, new(code, message), 401);
    public static Result<T> Forbidden(string code, string message) => new(false, default, new(code, message), 403);
    public static Result<T> NotFound(string code, string message) => new(false, default, new(code, message), 404);
    public static Result<T> Conflict(string code, string message) => new(false, default, new(code, message), 409);

    public static Result<T> ValidationFailure(
        string message,
        IReadOnlyDictionary<string, string[]> errors) =>
        new(false, default, new ResultError("VALIDATION_ERRORS", message, errors), 400);
}
