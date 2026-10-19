namespace FurniSpace.Domain.Entities;

public sealed class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid ShippingAddressId { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;
    public string ShippingAddressSnapshot { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset? PaidAt { get; set; }
    public User User { get; set; } = null!;
    public UserAddress ShippingAddress { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    public ICollection<Shipment> Shipments { get; set; } = [];
}
