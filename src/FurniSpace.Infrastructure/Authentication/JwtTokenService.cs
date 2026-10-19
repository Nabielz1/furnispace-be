using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FurniSpace.Application.Common.Services;
using FurniSpace.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FurniSpace.Infrastructure.Authentication;

public sealed class JwtTokenService(
    IConfiguration configuration,
    ISystemDateTimeProvider dateTimeProvider) : IJwtTokenService
{
    public AccessTokenResult CreateAccessToken(User user)
    {
        var secretKey = configuration["TokenAuthentication:SecretKey"];
        if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
        {
            throw new InvalidOperationException("TokenAuthentication:SecretKey must contain at least 32 characters.");
        }

        var issuer = configuration["TokenAuthentication:Issuer"] ?? "FurniSpace";
        var audience = configuration["TokenAuthentication:Audience"] ?? "FurniSpace.Web";
        var expiresInMinutes = configuration.GetValue("TokenAuthentication:Expires", 15);
        var now = dateTimeProvider.UtcNow;
        var expiresAt = now.AddMinutes(expiresInMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            now.UtcDateTime,
            expiresAt.UtcDateTime,
            credentials);

        return new AccessTokenResult(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
