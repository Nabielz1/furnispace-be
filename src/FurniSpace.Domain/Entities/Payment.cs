namespace FurniSpace.Domain.Entities;

public sealed class Payment : BaseEntity
{
    public Guid OrderId { get; set; }
    public string PaymentReference { get; set; } = string.Empty;
    public string? GatewayTransactionId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? PaymentUrl { get; set; }
    public DateTimeOffset? ExpiredAt { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
    public Order Order { get; set; } = null!;
    public ICollection<PaymentEvent> Events { get; set; } = [];
}
