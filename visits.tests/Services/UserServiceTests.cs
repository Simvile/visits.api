using Moq;
using visits.api.Configurations;
using visits.api.Services;
using visits.tests.Common.Factories;
using visits.tests.Common.Fixtures;

namespace visits.tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task GetUserProfile_Should_Return_UserProfile()
    {
        // Arrange
        var dbContext = new DatabaseFixture();
        var user = UserFactory.Create();

        // (Optional but recommended) seed test data
        dbContext.Context.Users.Add(user);
        await dbContext.Context.SaveChangesAsync();
        
        var userContext = new Mock<IUserContext>();
        userContext.Setup(u => u.UserId).Returns(user.Id);
        userContext.Setup(u => u.Roles).Returns(new List<string>());
        

        var service = new UserService(dbContext.Context, userContext.Object);

        // Act
        var result = await service.GetUserProfileAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.Fullname);
        Assert.Equal("test@test.com", result.Email);
    }
}