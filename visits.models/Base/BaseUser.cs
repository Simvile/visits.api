using Microsoft.AspNetCore.Identity;
using visits.models.Core;

namespace visits.models.Base;

public class BaseUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;
    public Guid UserTypeId { get; set; }
    public ClassificationValues UserType { get; set; } = null!;
    public bool PasswordExpires { get; set; }
    public DateTime? PasswordExpiryDate { get; set; }
    public Guid RoleId { get; set; }
    public ClassificationValues Role { get; set; } = null!;
    
    public Guid InstitutionId { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
}