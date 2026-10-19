using FurniSpace.Application.Features.Auth;
using FurniSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FurniSpace.Infrastructure.Persistence.Repositories;

public sealed class AuthRepository(AppDbContext dbContext) : IAuthRepository
{
    public Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken cancellationToken = default) =>
        dbContext.Users.AnyAsync(x => x.Email == normalizedEmail, cancellationToken);

    public Task<User?> GetUserByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default) =>
        dbContext.Users.SingleOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);

    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

    public Task<RefreshToken?> GetRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default) =>
        dbContext.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

    public void AddUser(User user) => dbContext.Users.Add(user);

    public void AddRefreshToken(RefreshToken refreshToken) => dbContext.RefreshTokens.Add(refreshToken);
}
