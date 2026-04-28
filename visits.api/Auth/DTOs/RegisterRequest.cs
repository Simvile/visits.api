using System.Reflection.Metadata.Ecma335;
using visits.api.Utils;

namespace visits.api.Auth.DTOs;

public class RegisterRequest
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DropdownModel Institution { get; set; } = null!;
    public DropdownModel UserType { get; set; } = null!;
    public DropdownModel Role { get; set; } = null!;
    public string? StudentNumber { get; set; }
    public string? IdentityNumber { get; set; }
    
}