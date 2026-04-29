namespace visits.api.DTOs;

public class UserProfile
{
    public string Username { get; set; } = null!;
    public Guid Id { get; set; }
    public string Fullname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string StudentNumber { get; set; } = null!;
}