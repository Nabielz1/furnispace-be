namespace FurniSpace.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Guid SellerId { get; set; }
    public Guid? VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public string? Sku { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public SellerProfile Seller { get; set; } = null!;
    public ProductVariant? Variant { get; set; }
    public Review? Review { get; set; }
}
