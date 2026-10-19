namespace FurniSpace.Domain.Entities;

public sealed class ProductVariantOption : BaseEntity
{
    public Guid VariantId { get; set; }
    public string OptionName { get; set; } = string.Empty;
    public string OptionValue { get; set; } = string.Empty;
    public ProductVariant Variant { get; set; } = null!;
}
