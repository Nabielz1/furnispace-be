using FurniSpace.Application.Common;

namespace FurniSpace.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result, HttpContext context)
    {
        if (result.IsSuccess)
        {
            var response = ApiResponse<T>.FromSuccess(result.Value!);
            return Results.Json(response, statusCode: result.StatusCode);
        }

        var error = result.Error!;
        return Results.Json(
            ApiResponse<T>.FromFailure(
                error.Code,
                error.Message,
                context.TraceIdentifier,
                error.ValidationErrors),
            statusCode: result.StatusCode);
    }
}
