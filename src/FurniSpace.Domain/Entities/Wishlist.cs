namespace FurniSpace.Domain.Entities;

public sealed class Wishlist : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public User User { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
