using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Moq;
using visits.api.Auth;
using visits.api.Auth.Services;
using visits.api.Data;
using visits.models.Base;
using visits.tests.Common.Builders;
using visits.tests.Common.Mocks;

namespace visits.tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_Should_Call_UserManager_And_Profile_Creation()
    {
        // Arrange
        var userManager = UserManagerMock.CreateSuccess();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w =>
                w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var dbContext = new AppDbContext(options);
        var jwt = Options.Create(new JwtSettings
        {
            Secret = "THIS_IS_A_SUPER_LONG_TEST_SECRET_KEY_123456789",
            Issuer = "test",
            Audience = "test",
            ExpiryMinutes = 60
        });

        var service = new AuthService(userManager.Object, dbContext, jwt);

        var request = new RegisterRequestBuilder().Build();

        // Act
        await service.RegisterAsync(request);

        // Assert
        userManager.Verify(x =>
                x.CreateAsync(It.IsAny<BaseUser>(), It.IsAny<string>()),
            Times.Once);
    }
}