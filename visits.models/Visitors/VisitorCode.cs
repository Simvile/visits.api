using System.ComponentModel.DataAnnotations.Schema;
using visits.models.Base;

namespace visits.models.Visitors;

public class VisitorCode:BaseTenant
{
    public Guid IssuedByUserId { get; set; }
    public BaseUser IssuedByUser { get; set; } = null!;
    public Guid VisitorId { get; set; }
    public Visitor Visitor { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    [NotMapped]
    public bool IsUsed => UsedAt.HasValue;
}