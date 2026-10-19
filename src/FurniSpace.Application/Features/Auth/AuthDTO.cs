namespace FurniSpace.Application.Features.Auth;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string FullName,
    string? PhoneNumber);

public sealed record LoginRequest(string Email, string Password);
public sealed record RefreshTokenRequest(string RefreshToken);
public sealed record LogoutRequest(string RefreshToken);

public sealed record AuthUserResponse(
    Guid Id,
    string Email,
    string FullName,
    string? PhoneNumber,
    string Role,
    string Status);

public sealed record AuthTokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresAt,
    AuthUserResponse User);
