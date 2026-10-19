using FurniSpace.Application.Common.Validator;
using FurniSpace.Application.Features.Auth;

namespace FurniSpace.UnitTests.Auth;

public sealed class AuthValidatorTests
{
    private readonly RegisterRequestValidator _validator = new();

    [Fact]
    public async Task Register_WithValidRequest_ShouldPassValidation()
    {
        var request = new RegisterRequest(
            "customer@example.com",
            "Password123",
            "Customer Name",
            "081234567890");

        var result = await _validator.ValidateAsync(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("PASSWORD123")]
    [InlineData("Password")]
    [InlineData("Short1")]
    public async Task Register_WithWeakPassword_ShouldFailValidation(string password)
    {
        var request = new RegisterRequest("customer@example.com", password, "Customer Name", null);

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(RegisterRequest.Password));
    }

    [Fact]
    public async Task Register_WithInvalidEmail_ShouldFailValidation()
    {
        var request = new RegisterRequest("invalid-email", "Password123", "Customer Name", null);

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(RegisterRequest.Email));
    }
}
