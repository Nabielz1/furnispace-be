namespace FurniSpace.Domain.Constants;

public static class ProductStatus
{
    public const string Draft = "draft";
    public const string PendingReview = "pending_review";
    public const string Active = "active";
    public const string Rejected = "rejected";
    public const string Inactive = "inactive";
    public const string Archived = "archived";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Draft, PendingReview, Active, Rejected, Inactive, Archived
    };
}
