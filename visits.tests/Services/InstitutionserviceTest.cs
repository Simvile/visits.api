using Moq;
using visits.api.DTOs;
using visits.api.Manager.Interfaces;
using visits.api.Services;
using visits.api.Utils;
using visits.models.Base;
using visits.models.Core;
using visits.tests.Common.Factories;
using visits.tests.Common.Fixtures;

namespace visits.tests.Services;

public class InstitutionserviceTest
{
    private static readonly BaseUser User = UserFactory.Create();
    private readonly UserContextFixture _userContext = new(User);
    private readonly Mock<IInstitutionManager> _manager = new();
    private readonly Mock<IClassificationManager> _classificationManager = new();
    private readonly Mock<IAddressManager> _addressManager = new();

    [Fact]
    public async Task SaveAsync_SuccessfullySavesInstitution()
    {
        // Arrange
        var masterObj = new InstitutionMaster
        {
            Id = Guid.NewGuid(),
            Name = "Master",
            Type = new DropdownModel
            {
                Id = Guid.NewGuid(),
                Code = "Master",
            },
            Address = new DropdownModel
            {
                Id = Guid.NewGuid(),
                Code = "MasterAddress",
            }
        };
        
        _manager.Setup(m => m.GetById(It.IsAny<Guid>())).ReturnsAsync((Institution)null);
        _manager.Setup(m => m.Validate(It.IsAny<Institution>())).ReturnsAsync(new ResponseHandler());
        _manager.Setup(m => m.Save(It.IsAny<Institution>())).ReturnsAsync(new ResponseHandler());
        
        var service = new InstitutionService(_manager.Object ,_userContext.Context.Object, _addressManager.Object,_classificationManager.Object);
        // Act
        
        var act = async () => await service.SaveAsync(masterObj);
        
        // Assert
        var result = await act();
        Assert.Equal(masterObj.Id, result.Id);
        Assert.False(result.HasErrorMessage);
    }
}