namespace FurniSpace.Domain.Entities;

public sealed class Review : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderItemId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string Status { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public OrderItem OrderItem { get; set; } = null!;
}
