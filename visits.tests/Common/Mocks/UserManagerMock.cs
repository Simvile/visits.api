using Microsoft.AspNetCore.Identity;
using Moq;
using visits.models.Base;

namespace visits.tests.Common.Mocks;

public static class UserManagerMock
{
    public static Mock<UserManager<BaseUser>> CreateSuccess()
    {
        var store = new Mock<IUserStore<BaseUser>>();
        var manager = new Mock<UserManager<BaseUser>>(
            store.Object, null, null, null, null, null, null, null, null);

        manager.Setup(x => x.CreateAsync(It.IsAny<BaseUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        manager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((BaseUser?)null);

        // Make sure this covers ALL BaseUser instances
        manager.Setup(x => x.GetRolesAsync(It.IsAny<BaseUser>()))
            .ReturnsAsync(new List<string>() as IList<string>);

        return manager;
    }
}