namespace FurniSpace.Domain.Entities;

public sealed class Product : BaseEntity
{
    public Guid SellerId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string? Material { get; set; }
    public decimal Weight { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Length { get; set; }
    public decimal? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public SellerProfile Seller { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<ProductImage> Images { get; set; } = [];
    public ICollection<ProductVariant> Variants { get; set; } = [];
    public ICollection<Inventory> Inventories { get; set; } = [];
    public ICollection<CartItem> CartItems { get; set; } = [];
    public ICollection<Wishlist> Wishlists { get; set; } = [];
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}
