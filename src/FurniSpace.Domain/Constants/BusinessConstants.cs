namespace FurniSpace.Domain.Constants;

public static class UserRole
{
    public const string Customer = "customer";
    public const string Seller = "seller";
    public const string Admin = "admin";
    public static readonly IReadOnlySet<string> All = Set(Customer, Seller, Admin);
    private static HashSet<string> Set(params string[] values) => new(values, StringComparer.OrdinalIgnoreCase);
}

public static class UserStatus
{
    public const string Active = "active";
    public const string Inactive = "inactive";
    public const string Suspended = "suspended";
    public static readonly IReadOnlySet<string> All = Set(Active, Inactive, Suspended);
    private static HashSet<string> Set(params string[] values) => new(values, StringComparer.OrdinalIgnoreCase);
}

public static class SellerStatus
{
    public const string PendingVerification = "pending_verification";
    public const string Active = "active";
    public const string Suspended = "suspended";
    public const string Rejected = "rejected";
    public static readonly IReadOnlySet<string> All = Set(PendingVerification, Active, Suspended, Rejected);
    private static HashSet<string> Set(params string[] values) => new(values, StringComparer.OrdinalIgnoreCase);
}

public static class ProductCondition
{
    public const string New = "new";
    public const string Used = "used";
    public static readonly IReadOnlySet<string> All = Set(New, Used);
    private static HashSet<string> Set(params string[] values) => new(values, StringComparer.OrdinalIgnoreCase);
}

public static class OrderStatus
{
    public const string PendingPayment = "pending_payment";
    public const string Paid = "paid";
    public const string Processing = "processing";
    public const string ReadyToShip = "ready_to_ship";
    public const string Shipped = "shipped";
    public const string Delivered = "delivered";
    public const string Completed = "completed";
    public const string Cancelled = "cancelled";
    public const string Refunded = "refunded";
    public static readonly IReadOnlySet<string> All = Set(PendingPayment, Paid, Processing, ReadyToShip, Shipped, Delivered, Completed, Cancelled, Refunded);
    private static HashSet<string> Set(params string[] values) => new(values, StringComparer.OrdinalIgnoreCase);
}

public static class PaymentStatus
{
    public const string Pending = "pending";
    public const string Processing = "processing";
    public const string Paid = "paid";
    public const string Failed = "failed";
    public const string Expired = "expired";
    public const string Cancelled = "cancelled";
    public const string Refunded = "refunded";
    public static readonly IReadOnlySet<string> All = Set(Pending, Processing, Paid, Failed, Expired, Cancelled, Refunded);
    private static HashSet<string> Set(params string[] values) => new(values, StringComparer.OrdinalIgnoreCase);
}

public static class ShipmentStatus
{
    public const string Pending = "pending";
    public const string ReadyToShip = "ready_to_ship";
    public const string Shipped = "shipped";
    public const string Delivered = "delivered";
    public const string Failed = "failed";
    public static readonly IReadOnlySet<string> All = Set(Pending, ReadyToShip, Shipped, Delivered, Failed);
    private static HashSet<string> Set(params string[] values) => new(values, StringComparer.OrdinalIgnoreCase);
}

public static class ReviewStatus
{
    public const string Pending = "pending";
    public const string Approved = "approved";
    public const string Rejected = "rejected";
    public static readonly IReadOnlySet<string> All = Set(Pending, Approved, Rejected);
    private static HashSet<string> Set(params string[] values) => new(values, StringComparer.OrdinalIgnoreCase);
}
