namespace FurniSpace.Domain.Entities;

public sealed class SellerProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string StoreSlug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = [];
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public ICollection<Shipment> Shipments { get; set; } = [];
}
