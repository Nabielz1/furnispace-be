using FurniSpace.Application.Common.Services;

namespace FurniSpace.Infrastructure.Authentication;

public sealed class SystemDateTimeProvider : ISystemDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
