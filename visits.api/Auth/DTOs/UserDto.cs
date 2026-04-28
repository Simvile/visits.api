namespace visits.api.Auth.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid InstitutionId { get; set; }
}