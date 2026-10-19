using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using FurniSpace.Application.Common;
using FurniSpace.Application.Common.Services;
using FurniSpace.Domain.Constants;
using FurniSpace.Domain.Entities;

namespace FurniSpace.Application.Features.Auth;

public sealed class AuthService(
    IAuthRepository repository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService,
    ISystemDateTimeProvider dateTimeProvider,
    IValidator<RegisterRequest> registerValidator,
    IValidator<LoginRequest> loginValidator,
    IValidator<RefreshTokenRequest> refreshValidator,
    IValidator<LogoutRequest> logoutValidator)
{
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);

    public async Task<Result<AuthTokenResponse>> RegisterAsync(
        RegisterRequest request,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var validation = await registerValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return ValidationFailure<AuthTokenResponse>(validation.Errors);
        }

        var email = NormalizeEmail(request.Email);
        if (await repository.EmailExistsAsync(email, cancellationToken))
        {
            return Result<AuthTokenResponse>.Conflict("EMAIL_ALREADY_REGISTERED", "Email is already registered.");
        }

        var now = dateTimeProvider.UtcNow;
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = request.FullName.Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim(),
            Role = UserRole.Customer,
            Status = UserStatus.Active,
            CreatedAt = now,
            CreatedBy = email
        };
        user.PasswordHash = passwordHasher.Hash(user, request.Password);

        repository.AddUser(user);
        var response = CreateTokenResponse(user, ipAddress, now);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result<AuthTokenResponse>.Created(response);
    }

    public async Task<Result<AuthTokenResponse>> LoginAsync(
        LoginRequest request,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var validation = await loginValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return ValidationFailure<AuthTokenResponse>(validation.Errors);
        }

        var user = await repository.GetUserByEmailAsync(NormalizeEmail(request.Email), cancellationToken);
        if (user is null || !passwordHasher.Verify(user, user.PasswordHash, request.Password))
        {
            return Result<AuthTokenResponse>.Unauthorized("INVALID_CREDENTIALS", "Email or password is invalid.");
        }

        if (!string.Equals(user.Status, UserStatus.Active, StringComparison.Ordinal))
        {
            return Result<AuthTokenResponse>.Forbidden("USER_INACTIVE", "User account is not active.");
        }

        var response = CreateTokenResponse(user, ipAddress, dateTimeProvider.UtcNow);
        await unitOfWork.SaveAsync(cancellationToken);
        return Result<AuthTokenResponse>.Success(response);
    }

    public async Task<Result<AuthTokenResponse>> RefreshAsync(
        RefreshTokenRequest request,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var validation = await refreshValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return ValidationFailure<AuthTokenResponse>(validation.Errors);
        }

        var token = await repository.GetRefreshTokenAsync(HashToken(request.RefreshToken), cancellationToken);
        var now = dateTimeProvider.UtcNow;
        if (token is null || token.RevokedAt is not null || token.ExpiresAt <= now || token.User.Status != UserStatus.Active)
        {
            return Result<AuthTokenResponse>.Unauthorized("INVALID_REFRESH_TOKEN", "Refresh token is invalid or expired.");
        }

        token.RevokedAt = now;
        token.UpdatedAt = now;
        token.UpdatedBy = token.User.Email;
        var response = CreateTokenResponse(token.User, ipAddress, now);
        await unitOfWork.SaveAsync(cancellationToken);
        return Result<AuthTokenResponse>.Success(response);
    }

    public async Task<Result<bool>> LogoutAsync(
        LogoutRequest request,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var validation = await logoutValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return ValidationFailure<bool>(validation.Errors);
        }

        var token = await repository.GetRefreshTokenAsync(HashToken(request.RefreshToken), cancellationToken);
        if (token is null || token.UserId != userId || token.RevokedAt is not null)
        {
            return Result<bool>.NotFound("REFRESH_TOKEN_NOT_FOUND", "Refresh token was not found.");
        }

        token.RevokedAt = dateTimeProvider.UtcNow;
        token.UpdatedAt = token.RevokedAt;
        token.UpdatedBy = token.User.Email;
        await unitOfWork.SaveAsync(cancellationToken);
        return Result<bool>.Success(true);
    }

    public async Task<Result<AuthUserResponse>> GetCurrentUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await repository.GetUserByIdAsync(userId, cancellationToken);
        return user is null
            ? Result<AuthUserResponse>.NotFound("USER_NOT_FOUND", "User was not found.")
            : Result<AuthUserResponse>.Success(MapUser(user));
    }

    private AuthTokenResponse CreateTokenResponse(User user, string? ipAddress, DateTimeOffset now)
    {
        var rawRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        repository.AddRefreshToken(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = HashToken(rawRefreshToken),
            ExpiresAt = now.Add(RefreshTokenLifetime),
            CreatedByIp = ipAddress,
            CreatedAt = now,
            CreatedBy = user.Email
        });

        var accessToken = jwtTokenService.CreateAccessToken(user);
        return new AuthTokenResponse(accessToken.Token, rawRefreshToken, accessToken.ExpiresAt, MapUser(user));
    }

    private static AuthUserResponse MapUser(User user) =>
        new(user.Id, user.Email, user.FullName, user.PhoneNumber, user.Role, user.Status);

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))).ToLowerInvariant();

    private static Result<T> ValidationFailure<T>(IEnumerable<FluentValidation.Results.ValidationFailure> failures) =>
        Result<T>.ValidationFailure(
            "One or more validation errors occurred.",
            failures.GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(error => error.ErrorMessage).Distinct().ToArray()));
}
