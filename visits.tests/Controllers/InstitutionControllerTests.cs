using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using visits.api.Controllers;
using visits.api.DTOs;
using visits.api.Interfaces;
using visits.api.Utils;

namespace visits.tests.Controllers;

public class InstitutionControllerTests
{
    private static readonly Mock<IInstitutionService> Service = new();
    private readonly InstitutionController _controller = new(Service.Object);

    [Fact]
    public async Task Save_ReturnsOk_WhenInstitutionServiceIsSaved()
    {
        // Arrange
        var institutionMaster = new InstitutionMaster();
        Service.Setup(s => s.SaveAsync(It.IsAny<InstitutionMaster>()))
            .ReturnsAsync(new ResponseHandler());
        
        // Act
        var act = async () => await _controller.Save(institutionMaster);

        // Assert
        var result = await act();
        Assert.NotNull(result);
        result.Should().BeOfType<OkObjectResult>();
        Service.Verify(s => s.SaveAsync(It.IsAny<InstitutionMaster>()), Times.Once);
    }
}