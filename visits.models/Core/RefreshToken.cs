using visits.models.Base;

namespace visits.models.Core;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public BaseUser User { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string JwtId { get; set; } = null!;
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime ExpiryAt { get; set; }
}