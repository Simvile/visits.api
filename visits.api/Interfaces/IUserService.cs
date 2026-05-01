using visits.api.DTOs;
using visits.api.Utils;

namespace visits.api.Interfaces;

public interface IUserService
{
    Task<UserProfile?> GetMyUserProfileAsync();
    Task<ResponseHandler> SaveUserProfileAsync(UserProfile userProfile);
    Task<UserProfile?> GetUserById(Guid userId);
}