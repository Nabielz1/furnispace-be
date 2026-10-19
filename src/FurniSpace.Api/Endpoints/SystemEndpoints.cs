using FurniSpace.Application.Common;
using FurniSpace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurniSpace.Api.Endpoints;

public static class SystemEndpoints
{
    public static RouteGroupBuilder MapSystemEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", (HttpContext context) => Results.Ok(
            ApiResponse<SystemInfoResponse>.FromSuccess(
                new SystemInfoResponse("FurniSpace API", "v1"))))
            .WithName("GetSystemInfo")
            .WithTags("System");

        group.MapGet("/health/database", async (AppDbContext dbContext, CancellationToken ct) =>
        {
            var canConnect = await dbContext.Database.CanConnectAsync(ct);
            return canConnect
                ? Results.Ok(new { status = "Healthy", database = "furnispace_db" })
                : Results.Problem(
                    title: "Database unavailable",
                    statusCode: StatusCodes.Status503ServiceUnavailable);
        })
            .WithName("GetDatabaseHealth")
            .WithTags("System");

        return group;
    }

    private sealed record SystemInfoResponse(string Name, string Version);
}
