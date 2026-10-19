namespace FurniSpace.Domain.Entities;

public sealed class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid? VariantId { get; set; }
    public int AvailableStock { get; set; }
    public int ReservedStock { get; set; }
    public int SoldStock { get; set; }
    public int Version { get; set; }
    public Product Product { get; set; } = null!;
    public ProductVariant? Variant { get; set; }
}
