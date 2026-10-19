using FurniSpace.Application.Common.Services;
using FurniSpace.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FurniSpace.Infrastructure.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(User user, string password) => _hasher.HashPassword(user, password);

    public bool Verify(User user, string passwordHash, string password) =>
        _hasher.VerifyHashedPassword(user, passwordHash, password) != PasswordVerificationResult.Failed;
}
