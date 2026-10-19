using Microsoft.Extensions.Configuration;

namespace FurniSpace.Infrastructure.Persistence;

public static class DatabaseConnectionStringProvider
{
    public static string Get(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        return string.IsNullOrWhiteSpace(connectionString)
            ? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' has not been configured.")
            : connectionString;
    }
}
