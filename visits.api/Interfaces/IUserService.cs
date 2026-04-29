using visits.api.DTOs;

namespace visits.api.Interfaces;

public interface IUserService
{
    Task<UserProfile?> GetUserProfileAsync();
}