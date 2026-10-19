using FurniSpace.Application.Common;

namespace FurniSpace.UnitTests.Common;

public sealed class PaginationDefaultsTests
{
    [Theory]
    [InlineData(null, null, 1, 10)]
    [InlineData(-1, 0, 1, 1)]
    [InlineData(2, 500, 2, 100)]
    public void Normalize_ShouldKeepValuesInsideSupportedRange(
        int? page,
        int? pageSize,
        int expectedPage,
        int expectedPageSize)
    {
        var result = PaginationDefaults.Normalize(page, pageSize);

        Assert.Equal(expectedPage, result.Page);
        Assert.Equal(expectedPageSize, result.PageSize);
    }
}
