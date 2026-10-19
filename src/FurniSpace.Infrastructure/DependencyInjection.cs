using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FurniSpace.Infrastructure.Persistence;
using FurniSpace.Application.Common;
using FurniSpace.Application.Common.Services;
using FurniSpace.Application.Features.Auth;
using FurniSpace.Infrastructure.Authentication;
using FurniSpace.Infrastructure.Persistence.Repositories;

namespace FurniSpace.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = DatabaseConnectionStringProvider.Get(configuration);

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsHistoryTable("__ef_migrations_history", "furnispace")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<ISystemDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}
