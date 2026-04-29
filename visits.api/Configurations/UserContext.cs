using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using visits.api.Utils;

namespace visits.api.Configurations;

public interface IUserContext
{
    Guid UserId { get; }
    Guid TenantId { get; }
    string FullName { get; }
    string Email { get; }
    string PhoneNumber { get; }
    DropdownModel Role { get; }
    IEnumerable<string> Roles { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
    UserContext? GetUserContext();
}

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public Guid UserId => Guid.TryParse(
        User?.FindFirstValue("user_id"), out var id) ? id : Guid.Empty;

    public Guid TenantId => Guid.TryParse(
        User?.FindFirstValue("tenant_id"), out var id) ? id : Guid.Empty;

    public string FullName => User?.FindFirstValue("fullName") ?? string.Empty;

    public string Email => User?.FindFirstValue(JwtRegisteredClaimNames.Email) ?? string.Empty;

    public string PhoneNumber => User?.FindFirstValue("phone_number") ?? string.Empty;

    // All roles from ClaimTypes.Role claims
    public IEnumerable<string> Roles =>
        User?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();

    // Primary role — first role in the list
    public DropdownModel Role
    {
        get
        {
            var primaryRole = Roles.FirstOrDefault() ?? string.Empty;

            return new DropdownModel
            {
                Id = Guid.Empty,
                Description = primaryRole
            };
        }
    }

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    // Convenience method to check a specific role
    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;

    public UserContext? GetUserContext()
    {
        if (!IsAuthenticated)
            return null;

        return new UserContext
        {
            UserId = UserId,
            TenantId = TenantId,
            FullName = FullName,
            Email = Email,
            Role = Role,
            Roles = Roles,
            PhoneNumber = PhoneNumber
        };
    }
}

public class UserContext
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DropdownModel Role { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    public string PhoneNumber { get; set; } = string.Empty;
}