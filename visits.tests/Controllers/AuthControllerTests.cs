using FluentAssertions;
using Moq;
using visits.api.Auth.DTOs;
using visits.api.Auth.Services;
using visits.api.Controllers;
using visits.tests.Common.Builders;

namespace visits.tests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_Should_Return_Ok()
    {
        // Arrange
        var service = new Mock<IAuthService>();

        service.Setup(x => x.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ReturnsAsync(new AuthResponse { AccessToken = "abc" });

        var controller = new AuthController(service.Object);

        var request = new RegisterRequestBuilder().Build();

        // Act
        var result = await controller.Register(request);

        // Assert
        result.Should().NotBeNull();
    }
}