using FurniSpace.Application.Common;

namespace FurniSpace.UnitTests.Common;

public sealed class ResultTests
{
    [Fact]
    public void Success_ShouldContainValueAndNoError()
    {
        var result = Result<string>.Success("ok");

        Assert.True(result.IsSuccess);
        Assert.Equal("ok", result.Value);
        Assert.Null(result.Error);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void NotFound_ShouldContainExpectedError()
    {
        var result = Result<string>.NotFound("PRODUCT_NOT_FOUND", "Product not found.");

        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("PRODUCT_NOT_FOUND", result.Error?.Code);
        Assert.Equal(404, result.StatusCode);
    }
}
