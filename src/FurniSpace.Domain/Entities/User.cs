namespace FurniSpace.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public ICollection<UserAddress> Addresses { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public SellerProfile? SellerProfile { get; set; }
    public Cart? Cart { get; set; }
    public ICollection<Wishlist> Wishlists { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}
