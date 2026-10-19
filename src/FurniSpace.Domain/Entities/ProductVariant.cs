namespace FurniSpace.Domain.Entities;

public sealed class ProductVariant : BaseEntity
{
    public Guid ProductId { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal PriceAdjustment { get; set; }
    public bool IsActive { get; set; } = true;
    public Product Product { get; set; } = null!;
    public ICollection<ProductVariantOption> Options { get; set; } = [];
    public ICollection<Inventory> Inventories { get; set; } = [];
    public ICollection<CartItem> CartItems { get; set; } = [];
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
