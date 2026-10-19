namespace FurniSpace.Domain.Entities;

public sealed class ProductImage : BaseEntity
{
    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
    public Product Product { get; set; } = null!;
}
