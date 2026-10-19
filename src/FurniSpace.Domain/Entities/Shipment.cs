namespace FurniSpace.Domain.Entities;

public sealed class Shipment : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid SellerId { get; set; }
    public string Courier { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
    public decimal ShippingCost { get; set; }
    public string Status { get; set; } = string.Empty;
    public Order Order { get; set; } = null!;
    public SellerProfile Seller { get; set; } = null!;
}
