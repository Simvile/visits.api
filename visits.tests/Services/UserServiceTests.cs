using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using visits.api.DTOs;
using visits.api.Services;
using visits.api.Utils;
using visits.models.Base;
using visits.tests.Common.Factories;
using visits.tests.Common.Fixtures;
using visits.tests.Common.Mocks;

namespace visits.tests.Services;

public class UserServiceTests
{
    private static readonly BaseUser User = UserFactory.Create();
    private readonly UserContextFixture _userContext = new(User);
    private readonly Mock<UserManager<BaseUser>> _userManager = UserManagerMock.CreateSuccess();
    private readonly DatabaseFixture _dbContext = new();
    
    #region GetUserProfile Tests
    [Fact]
    public async Task GetUserProfile_Should_Return_UserProfile()
    {
        // Arrange
        _dbContext.Context.Users.Add(User);
        await _dbContext.Context.SaveChangesAsync();
        
        
        _userContext.Context.Setup(u => u.Roles).Returns(new List<string>());
        

        var service = new UserService(_dbContext.Context, _userContext.Context.Object, _userManager.Object);

        // Act
        var result = await service.GetMyUserProfileAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(User.Id, result.Id);
    }
    #endregion

    #region SaveUserProfile Tests
    [Fact]
    public async Task SaveUserProfile_ReturnsSucessMessageHandler_IfProfileIsValid()
    {
        //Arrange
        //var _user = UserFactory.Create();
        var userProfile = new UserProfile
        {
            Email =  "testing@data.com",
            PhoneNumber = "0711230987",
            Fullname = "Testing User",
            Id =  User.Id,
        };
        
        _dbContext.Context.Users.Add(User);
        await _dbContext.Context.SaveChangesAsync();
        
        _userContext.Context.Setup(u => u.UserId).Returns(User.Id);
        _userContext.Context.Setup(u => u.Email).Returns(User.Email!);
        
        _userManager.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(User);
        _userManager.Setup(u => u.UpdateAsync(It.IsAny<BaseUser>())).ReturnsAsync(IdentityResult.Success);
        
        var service = new UserService(_dbContext.Context, _userContext.Context.Object,  _userManager.Object);
        
        // Act
        var result = await service.SaveUserProfileAsync(userProfile);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(ResponseType.SuccessMessage, result.Type);
        
        result.Should().BeOfType(typeof(ResponseHandler));
        
        _userContext.Context.Verify(u => u.Email, Times.Once);
        _userManager.Verify(u => u.UpdateAsync(It.IsAny<BaseUser>()), Times.Once);
    }

    [Fact]
    public async Task SaveUserProfile_ThrowsException_IfProfileIsInvalid()
    {
        //Arrange 
        var userProfile = new UserProfile
        {
            Email =  "testing@data.com",
            PhoneNumber = "0711230987",
            Fullname = "Testing User",
            Id =  User.Id,
        };
        
        _userContext.Context.Setup(u => u.Email).Returns(User.Email!);
        
        _userManager.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((BaseUser)null!);
        
        var service = new UserService(_dbContext.Context, _userContext.Context.Object,  _userManager.Object);
        
        //Act
        Func<Task> act = async () => await service.SaveUserProfileAsync(userProfile);
        
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User not found");
        
        _userContext.Context.Verify(u => u.Email, Times.Once);
        _userManager.Verify(u => u.UpdateAsync(It.IsAny<BaseUser>()), Times.Never);
    }
    
    #endregion
    
    #region GetUserById Tests
    [Fact]
    public async Task GetUserById_ReturnsUser_IfIdIsValid()
    {
        //Arrange
        _userManager.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(User);
        
        var service = new UserService(_dbContext.Context, _userContext.Context.Object,  _userManager.Object);
        
        // Act
        var coreUser = await service.GetUserById(User.Id);
        
        // Assert
        Assert.NotNull(User);
        coreUser.Should().BeOfType(typeof(UserProfile));
        Assert.Equal(User.Id, coreUser.Id);
        
        _userManager.Verify(u => u.FindByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetUserById_ThrowsException_IfBaseUserIsNull()
    {
        // Arrange
         _userManager.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((BaseUser)null!);
         var service = new UserService(_dbContext.Context, _userContext.Context.Object, _userManager.Object);
         
         // Act
         var act = async () => await service.GetUserById(UserServiceTests.User.Id);

         // Assert
         await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid User Id");
         _userManager.Verify(u => u.FindByIdAsync(It.IsAny<string>()), Times.Once);
    }
    
    #endregion
}