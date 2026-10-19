using FurniSpace.Api.Extensions;
using FurniSpace.Application.Features.Auth;
using Microsoft.AspNetCore.RateLimiting;

namespace FurniSpace.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder root)
    {
        var group = root.MapGroup("/auth")
            .WithTags("FurniSpace-Auth")
            .RequireRateLimiting("auth");

        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);
        group.MapPost("/refresh", RefreshAsync);
        group.MapPost("/logout", LogoutAsync).RequireAuthorization();
        group.MapGet("/me", GetCurrentUserAsync).RequireAuthorization();

        return root;
    }

    private static async Task<IResult> RegisterAsync(
        RegisterRequest request,
        AuthService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.RegisterAsync(request, context.Connection.RemoteIpAddress?.ToString(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        AuthService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.LoginAsync(request, context.Connection.RemoteIpAddress?.ToString(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> RefreshAsync(
        RefreshTokenRequest request,
        AuthService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.RefreshAsync(request, context.Connection.RemoteIpAddress?.ToString(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> LogoutAsync(
        LogoutRequest request,
        AuthService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.LogoutAsync(request, context.User.GetUserId(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetCurrentUserAsync(
        AuthService service,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await service.GetCurrentUserAsync(context.User.GetUserId(), cancellationToken);
        return result.ToHttpResult(context);
    }
}
