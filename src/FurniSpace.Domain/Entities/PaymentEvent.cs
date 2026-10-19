namespace FurniSpace.Domain.Entities;

public sealed class PaymentEvent : BaseEntity
{
    public Guid PaymentId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string GatewayEventId { get; set; } = string.Empty;
    public string? GatewayTransactionId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTimeOffset? ProcessedAt { get; set; }
    public Payment Payment { get; set; } = null!;
}
