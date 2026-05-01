using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using visits.api.Configurations;
using visits.api.Data;
using visits.api.DTOs;
using visits.api.Interfaces;
using visits.api.Utils;
using visits.models.Base;

namespace visits.api.Services;

public class UserService(AppDbContext context, IUserContext userContext, UserManager<BaseUser> userManagerObject): IUserService
{
    public async Task<UserProfile?> GetMyUserProfileAsync()
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

    public async Task<ResponseHandler> SaveUserProfileAsync(UserProfile userProfile)
    {
        var responseHandler = new ResponseHandler();
        
        // let's get the user
        var user = await userManagerObject.FindByIdAsync(userProfile.Id.ToString());
        
        if (user is null)
            throw new InvalidOperationException("User not found");
        
        user.UserName = userProfile.Username;
        user.FullName = userProfile.Fullname;
        user.Email = userProfile.Email;
        user.PhoneNumber = userProfile.PhoneNumber;
        
        var updateResult = await userManagerObject.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var erros = updateResult.Errors.Select(e => e.Description).ToList();
            erros.ForEach(e => responseHandler.AddMessage(e, ResponseType.ErrorMessage));
        }
        else
        {
            responseHandler.AddMessage("Successfully updated profile", ResponseType.SuccessMessage);
        }
        
        return responseHandler;
    }

    public async Task<UserProfile?> GetUserById(Guid userId)
    {
        var baseUser = await userManagerObject.FindByIdAsync(userId.ToString());
        
        if (baseUser is null)
            throw new ArgumentException("Invalid User Id");
        
        return new UserProfile
        {
            Id =  baseUser.Id,
            Fullname = baseUser.FullName,
            Email = baseUser.Email!,
            PhoneNumber = baseUser.PhoneNumber!,
            Username =  baseUser.UserName!,
        };
    }
}