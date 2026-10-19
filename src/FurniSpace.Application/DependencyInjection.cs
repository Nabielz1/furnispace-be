using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FurniSpace.Application.Features.Auth;

namespace FurniSpace.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<AuthService>();
        return services;
    }
}
