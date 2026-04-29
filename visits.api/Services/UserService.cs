using Microsoft.EntityFrameworkCore;
using visits.api.Configurations;
using visits.api.Data;
using visits.api.DTOs;
using visits.api.Interfaces;

namespace visits.api.Services;

public class UserService(AppDbContext context, IUserContext userContext): IUserService
{
    public async Task<UserProfile?> GetUserProfileAsync()
    {
        var user = await context.Users
            .Where(x => x.Id == userContext.UserId)
            .FirstOrDefaultAsync();
        
        if (user is null)
            return null;
        
        return new UserProfile
        {
            Id = user.Id,
            Fullname = user.FullName,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
        };
    }
}