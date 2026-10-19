using FurniSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniSpace.Infrastructure.Persistence.EntityConfigs;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.CreatedBy).HasColumnName("created_by").HasMaxLength(255);
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.UpdatedBy).HasColumnName("updated_by").HasMaxLength(255);
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(x => x.DeletedAt).HasColumnName("deleted_at");
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

public sealed class UserConfig : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.ToTable("users");
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(500).IsRequired();
        builder.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasMaxLength(30);
        builder.Property(x => x.Role).HasColumnName("role").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}

public sealed class UserAddressConfig : BaseEntityConfiguration<UserAddress>
{
    public override void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        base.Configure(builder);
        builder.ToTable("user_addresses");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.Label).HasColumnName("label").HasMaxLength(80).IsRequired();
        builder.Property(x => x.RecipientName).HasColumnName("recipient_name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasMaxLength(30).IsRequired();
        builder.Property(x => x.AddressLine).HasColumnName("address_line").IsRequired();
        builder.Property(x => x.Province).HasColumnName("province").HasMaxLength(100).IsRequired();
        builder.Property(x => x.City).HasColumnName("city").HasMaxLength(100).IsRequired();
        builder.Property(x => x.District).HasColumnName("district").HasMaxLength(100).IsRequired();
        builder.Property(x => x.PostalCode).HasColumnName("postal_code").HasMaxLength(10).IsRequired();
        builder.Property(x => x.IsDefault).HasColumnName("is_default").HasDefaultValue(false);
        builder.HasOne(x => x.User).WithMany(x => x.Addresses).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.UserId);
    }
}

public sealed class RefreshTokenConfig : BaseEntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);
        builder.ToTable("refresh_tokens");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.TokenHash).HasColumnName("token_hash").HasMaxLength(500).IsRequired();
        builder.Property(x => x.ExpiresAt).HasColumnName("expires_at");
        builder.Property(x => x.RevokedAt).HasColumnName("revoked_at");
        builder.Property(x => x.CreatedByIp).HasColumnName("created_by_ip").HasMaxLength(64);
        builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.TokenHash).IsUnique();
    }
}

public sealed class SellerProfileConfig : BaseEntityConfiguration<SellerProfile>
{
    public override void Configure(EntityTypeBuilder<SellerProfile> builder)
    {
        base.Configure(builder);
        builder.ToTable("seller_profiles");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.StoreName).HasColumnName("store_name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.StoreSlug).HasColumnName("store_slug").HasMaxLength(220).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.LogoUrl).HasColumnName("logo_url").HasMaxLength(1000);
        builder.Property(x => x.BannerUrl).HasColumnName("banner_url").HasMaxLength(1000);
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.HasOne(x => x.User).WithOne(x => x.SellerProfile).HasForeignKey<SellerProfile>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasIndex(x => x.StoreSlug).IsUnique();
    }
}

public sealed class CategoryConfig : BaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);
        builder.ToTable("categories");
        builder.Property(x => x.ParentId).HasColumnName("parent_id");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Slug).HasColumnName("slug").HasMaxLength(180).IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.Slug).IsUnique();
    }
}

public sealed class ProductConfig : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);
        builder.ToTable("products", table =>
        {
            table.HasCheckConstraint("ck_products_base_price", "base_price >= 0");
            table.HasCheckConstraint("ck_products_dimensions", "weight >= 0 AND width >= 0 AND height >= 0 AND length >= 0");
            table.HasCheckConstraint("ck_products_rating", "average_rating IS NULL OR (average_rating >= 0 AND average_rating <= 5)");
            table.HasCheckConstraint("ck_products_review_count", "review_count >= 0");
        });
        builder.Property(x => x.SellerId).HasColumnName("seller_id");
        builder.Property(x => x.CategoryId).HasColumnName("category_id");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(x => x.Slug).HasColumnName("slug").HasMaxLength(280).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").IsRequired();
        builder.Property(x => x.BasePrice).HasColumnName("base_price").HasPrecision(18, 2);
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Condition).HasColumnName("condition").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Material).HasColumnName("material").HasMaxLength(100);
        builder.Property(x => x.Weight).HasColumnName("weight").HasPrecision(12, 3);
        builder.Property(x => x.Width).HasColumnName("width").HasPrecision(12, 2);
        builder.Property(x => x.Height).HasColumnName("height").HasPrecision(12, 2);
        builder.Property(x => x.Length).HasColumnName("length").HasPrecision(12, 2);
        builder.Property(x => x.AverageRating).HasColumnName("average_rating").HasPrecision(3, 2);
        builder.Property(x => x.ReviewCount).HasColumnName("review_count").HasDefaultValue(0);
        builder.HasOne(x => x.Seller).WithMany(x => x.Products).HasForeignKey(x => x.SellerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.HasIndex(x => x.SellerId);
        builder.HasIndex(x => x.CategoryId);
        builder.HasIndex(x => x.Status);
    }
}

public sealed class ProductImageConfig : BaseEntityConfiguration<ProductImage>
{
    public override void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        base.Configure(builder);
        builder.ToTable("product_images");
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.Property(x => x.ImageUrl).HasColumnName("image_url").HasMaxLength(1000).IsRequired();
        builder.Property(x => x.SortOrder).HasColumnName("sort_order");
        builder.Property(x => x.IsPrimary).HasColumnName("is_primary");
        builder.HasOne(x => x.Product).WithMany(x => x.Images).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.ProductId);
    }
}

public sealed class ProductVariantConfig : BaseEntityConfiguration<ProductVariant>
{
    public override void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        base.Configure(builder);
        builder.ToTable("product_variants", table => table.HasCheckConstraint("ck_product_variants_price_adjustment", "price_adjustment >= 0"));
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.Property(x => x.VariantName).HasColumnName("variant_name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Sku).HasColumnName("sku").HasMaxLength(100).IsRequired();
        builder.Property(x => x.PriceAdjustment).HasColumnName("price_adjustment").HasPrecision(18, 2);
        builder.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.HasOne(x => x.Product).WithMany(x => x.Variants).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.Sku).IsUnique();
        builder.HasIndex(x => x.ProductId);
    }
}

public sealed class ProductVariantOptionConfig : BaseEntityConfiguration<ProductVariantOption>
{
    public override void Configure(EntityTypeBuilder<ProductVariantOption> builder)
    {
        base.Configure(builder);
        builder.ToTable("product_variant_options");
        builder.Property(x => x.VariantId).HasColumnName("variant_id");
        builder.Property(x => x.OptionName).HasColumnName("option_name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.OptionValue).HasColumnName("option_value").HasMaxLength(150).IsRequired();
        builder.HasOne(x => x.Variant).WithMany(x => x.Options).HasForeignKey(x => x.VariantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.VariantId, x.OptionName }).IsUnique();
    }
}

public sealed class InventoryConfig : BaseEntityConfiguration<Inventory>
{
    public override void Configure(EntityTypeBuilder<Inventory> builder)
    {
        base.Configure(builder);
        builder.ToTable("inventories", table => table.HasCheckConstraint(
            "ck_inventories_stock",
            "available_stock >= 0 AND reserved_stock >= 0 AND sold_stock >= 0"));
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.Property(x => x.VariantId).HasColumnName("variant_id");
        builder.Property(x => x.AvailableStock).HasColumnName("available_stock");
        builder.Property(x => x.ReservedStock).HasColumnName("reserved_stock");
        builder.Property(x => x.SoldStock).HasColumnName("sold_stock");
        builder.Property(x => x.Version).HasColumnName("version").IsConcurrencyToken();
        builder.HasOne(x => x.Product).WithMany(x => x.Inventories).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Variant).WithMany(x => x.Inventories).HasForeignKey(x => x.VariantId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.ProductId, x.VariantId }).IsUnique();
        builder.HasIndex(x => x.ProductId).IsUnique().HasFilter("variant_id IS NULL");
    }
}

public sealed class CartConfig : BaseEntityConfiguration<Cart>
{
    public override void Configure(EntityTypeBuilder<Cart> builder)
    {
        base.Configure(builder);
        builder.ToTable("carts");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.HasOne(x => x.User).WithOne(x => x.Cart).HasForeignKey<Cart>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.UserId).IsUnique();
    }
}

public sealed class CartItemConfig : BaseEntityConfiguration<CartItem>
{
    public override void Configure(EntityTypeBuilder<CartItem> builder)
    {
        base.Configure(builder);
        builder.ToTable("cart_items", table => table.HasCheckConstraint("ck_cart_items_quantity", "quantity > 0"));
        builder.Property(x => x.CartId).HasColumnName("cart_id");
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.Property(x => x.VariantId).HasColumnName("variant_id");
        builder.Property(x => x.Quantity).HasColumnName("quantity");
        builder.HasOne(x => x.Cart).WithMany(x => x.Items).HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Product).WithMany(x => x.CartItems).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Variant).WithMany(x => x.CartItems).HasForeignKey(x => x.VariantId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.CartId);
    }
}

public sealed class WishlistConfig : BaseEntityConfiguration<Wishlist>
{
    public override void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        base.Configure(builder);
        builder.ToTable("wishlists");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.HasOne(x => x.User).WithMany(x => x.Wishlists).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Product).WithMany(x => x.Wishlists).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.UserId, x.ProductId }).IsUnique();
    }
}

public sealed class OrderConfig : BaseEntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);
        builder.ToTable("orders", table =>
        {
            table.HasCheckConstraint("ck_orders_subtotal", "subtotal >= 0");
            table.HasCheckConstraint("ck_orders_shipping_cost", "shipping_cost >= 0");
            table.HasCheckConstraint("ck_orders_discount_amount", "discount_amount >= 0");
            table.HasCheckConstraint("ck_orders_total_amount", "total_amount >= 0");
        });
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.OrderNumber).HasColumnName("order_number").HasMaxLength(80).IsRequired();
        builder.Property(x => x.ShippingAddressId).HasColumnName("shipping_address_id");
        builder.Property(x => x.RecipientName).HasColumnName("recipient_name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.RecipientPhone).HasColumnName("recipient_phone").HasMaxLength(30).IsRequired();
        builder.Property(x => x.ShippingAddressSnapshot).HasColumnName("shipping_address_snapshot").HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.Subtotal).HasColumnName("subtotal").HasPrecision(18, 2);
        builder.Property(x => x.ShippingCost).HasColumnName("shipping_cost").HasPrecision(18, 2);
        builder.Property(x => x.DiscountAmount).HasColumnName("discount_amount").HasPrecision(18, 2);
        builder.Property(x => x.TotalAmount).HasColumnName("total_amount").HasPrecision(18, 2);
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(x => x.PaidAt).HasColumnName("paid_at");
        builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ShippingAddress).WithMany(x => x.Orders).HasForeignKey(x => x.ShippingAddressId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.OrderNumber).IsUnique();
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Status);
    }
}

public sealed class OrderItemConfig : BaseEntityConfiguration<OrderItem>
{
    public override void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        base.Configure(builder);
        builder.ToTable("order_items", table =>
        {
            table.HasCheckConstraint("ck_order_items_quantity", "quantity > 0");
            table.HasCheckConstraint("ck_order_items_unit_price", "unit_price >= 0");
            table.HasCheckConstraint("ck_order_items_subtotal", "subtotal >= 0");
        });
        builder.Property(x => x.OrderId).HasColumnName("order_id");
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.Property(x => x.SellerId).HasColumnName("seller_id");
        builder.Property(x => x.VariantId).HasColumnName("variant_id");
        builder.Property(x => x.ProductName).HasColumnName("product_name").HasMaxLength(250).IsRequired();
        builder.Property(x => x.VariantName).HasColumnName("variant_name").HasMaxLength(200);
        builder.Property(x => x.Sku).HasColumnName("sku").HasMaxLength(100);
        builder.Property(x => x.Quantity).HasColumnName("quantity");
        builder.Property(x => x.UnitPrice).HasColumnName("unit_price").HasPrecision(18, 2);
        builder.Property(x => x.Subtotal).HasColumnName("subtotal").HasPrecision(18, 2);
        builder.HasOne(x => x.Order).WithMany(x => x.Items).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Product).WithMany(x => x.OrderItems).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Seller).WithMany(x => x.OrderItems).HasForeignKey(x => x.SellerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Variant).WithMany(x => x.OrderItems).HasForeignKey(x => x.VariantId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.OrderId);
    }
}

public sealed class PaymentConfig : BaseEntityConfiguration<Payment>
{
    public override void Configure(EntityTypeBuilder<Payment> builder)
    {
        base.Configure(builder);
        builder.ToTable("payments", table => table.HasCheckConstraint("ck_payments_amount", "amount >= 0"));
        builder.Property(x => x.OrderId).HasColumnName("order_id");
        builder.Property(x => x.PaymentReference).HasColumnName("payment_reference").HasMaxLength(120).IsRequired();
        builder.Property(x => x.GatewayTransactionId).HasColumnName("gateway_transaction_id").HasMaxLength(200);
        builder.Property(x => x.Provider).HasColumnName("provider").HasMaxLength(50).IsRequired();
        builder.Property(x => x.PaymentMethod).HasColumnName("payment_method").HasMaxLength(80).IsRequired();
        builder.Property(x => x.Amount).HasColumnName("amount").HasPrecision(18, 2);
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(x => x.PaymentUrl).HasColumnName("payment_url");
        builder.Property(x => x.ExpiredAt).HasColumnName("expired_at");
        builder.Property(x => x.PaidAt).HasColumnName("paid_at");
        builder.HasOne(x => x.Order).WithMany(x => x.Payments).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.PaymentReference).IsUnique();
        builder.HasIndex(x => x.OrderId);
        builder.HasIndex(x => x.GatewayTransactionId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.OrderId).IsUnique().HasFilter("status = 'paid'").HasDatabaseName("ux_payments_order_paid");
    }
}

public sealed class PaymentEventConfig : BaseEntityConfiguration<PaymentEvent>
{
    public override void Configure(EntityTypeBuilder<PaymentEvent> builder)
    {
        base.Configure(builder);
        builder.ToTable("payment_events");
        builder.Property(x => x.PaymentId).HasColumnName("payment_id");
        builder.Property(x => x.Provider).HasColumnName("provider").HasMaxLength(50).IsRequired();
        builder.Property(x => x.GatewayEventId).HasColumnName("gateway_event_id").HasMaxLength(200).IsRequired();
        builder.Property(x => x.GatewayTransactionId).HasColumnName("gateway_transaction_id").HasMaxLength(200);
        builder.Property(x => x.EventType).HasColumnName("event_type").HasMaxLength(100).IsRequired();
        builder.Property(x => x.ProcessedAt).HasColumnName("processed_at");
        builder.HasOne(x => x.Payment).WithMany(x => x.Events).HasForeignKey(x => x.PaymentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.Provider, x.GatewayEventId }).IsUnique();
    }
}

public sealed class ShipmentConfig : BaseEntityConfiguration<Shipment>
{
    public override void Configure(EntityTypeBuilder<Shipment> builder)
    {
        base.Configure(builder);
        builder.ToTable("shipments", table => table.HasCheckConstraint("ck_shipments_shipping_cost", "shipping_cost >= 0"));
        builder.Property(x => x.OrderId).HasColumnName("order_id");
        builder.Property(x => x.SellerId).HasColumnName("seller_id");
        builder.Property(x => x.Courier).HasColumnName("courier").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Service).HasColumnName("service").HasMaxLength(100).IsRequired();
        builder.Property(x => x.TrackingNumber).HasColumnName("tracking_number").HasMaxLength(200);
        builder.Property(x => x.ShippingCost).HasColumnName("shipping_cost").HasPrecision(18, 2);
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.HasOne(x => x.Order).WithMany(x => x.Shipments).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Seller).WithMany(x => x.Shipments).HasForeignKey(x => x.SellerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.OrderId, x.SellerId }).IsUnique();
    }
}

public sealed class ReviewConfig : BaseEntityConfiguration<Review>
{
    public override void Configure(EntityTypeBuilder<Review> builder)
    {
        base.Configure(builder);
        builder.ToTable("reviews", table => table.HasCheckConstraint("ck_reviews_rating", "rating BETWEEN 1 AND 5"));
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.Property(x => x.OrderItemId).HasColumnName("order_item_id");
        builder.Property(x => x.Rating).HasColumnName("rating");
        builder.Property(x => x.Comment).HasColumnName("comment");
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.HasOne(x => x.User).WithMany(x => x.Reviews).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany(x => x.Reviews).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.OrderItem).WithOne(x => x.Review).HasForeignKey<Review>(x => x.OrderItemId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.OrderItemId).IsUnique();
        builder.HasIndex(x => x.ProductId);
    }
}
