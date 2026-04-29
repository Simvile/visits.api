using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using visits.api.Controllers;
using visits.api.DTOs;
using visits.api.Interfaces;

namespace visits.tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task GetUserProfile_Should_Return_Ok()
    {
        // Arrange
        var service = new Mock<IUserService>();
        
        service.Setup(x => x.GetUserProfileAsync())
            .ReturnsAsync(new UserProfile { Email = "123" });

        var controller = new UserController(service.Object);
        
        //Act
        var result = await controller.GetProfile();
        
        // Assess
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }
}