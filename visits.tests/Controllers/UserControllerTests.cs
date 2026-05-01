using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using visits.api.Controllers;
using visits.api.DTOs;
using visits.api.Interfaces;

namespace visits.tests.Controllers;

public class UserControllerTests
{
    private static readonly Mock<IUserService> Service = new();
    private readonly UserController _controller = new(Service.Object);
    
    #region GetUserProfile Tests
    [Fact]
    public async Task GetUserProfile_Should_Return_Ok()
    {
        // Arrange
        Service.Setup(x => x.GetMyUserProfileAsync())
            .ReturnsAsync(new UserProfile { Email = "123" });
        
        //Act
        var result = await _controller.GetProfile();
        
        // Assess
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        Service.Verify(x => x.GetMyUserProfileAsync(), Times.Once);
    }
    #endregion
    
    #region GetUserById Tests
    [Fact]
    public async Task GetUserById_ShouldReturnUserProfile_IfUserExists()
    {
        // Arrange
        var guid = Guid.NewGuid();
        Service.Setup(s => s.GetUserById(It.IsAny<Guid>()))
            .ReturnsAsync(new UserProfile { Email = "123" })
            .Verifiable();
        
        // Act
        var act = async () => await _controller.GetUserById(guid);
        
        // Assert
        var result = await act();
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        
        Service.Verify(x => x.GetUserById(It.IsAny<Guid>()), Times.Once);
    }
    
    #endregion
}