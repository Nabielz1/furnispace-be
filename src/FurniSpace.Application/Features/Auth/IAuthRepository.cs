using FurniSpace.Domain.Entities;

namespace FurniSpace.Application.Features.Auth;

public interface IAuthRepository
{
    Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default);
    void AddUser(User user);
    void AddRefreshToken(RefreshToken refreshToken);
}
