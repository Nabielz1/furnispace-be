namespace FurniSpace.Domain.Entities;

public sealed class UserAddress : BaseEntity
{
    public Guid UserId { get; set; }
    public string Label { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = [];
}
