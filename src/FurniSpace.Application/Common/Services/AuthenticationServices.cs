using FurniSpace.Domain.Entities;

namespace FurniSpace.Application.Common.Services;

public interface ISystemDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}

public interface IPasswordHasher
{
    string Hash(User user, string password);
    bool Verify(User user, string passwordHash, string password);
}

public sealed record AccessTokenResult(string Token, DateTimeOffset ExpiresAt);

public interface IJwtTokenService
{
    AccessTokenResult CreateAccessToken(User user);
}
