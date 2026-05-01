using Moq;
using visits.api.Configurations;
using visits.models.Base;

namespace visits.tests.Common.Fixtures;

public class UserContextFixture
{
    public Mock<IUserContext> Context { get; }
    
    public UserContextFixture(BaseUser user)
    {
        var userContext = new Mock<IUserContext>();
        
        userContext.Setup(u => u.UserId).Returns(user.Id);
        userContext.Setup(u => u.Email).Returns(user.Email!);
        userContext.Setup(u => u.FullName).Returns(user.FullName);
        userContext.Setup(u => u.PhoneNumber).Returns(user.PhoneNumber!);
        
        Context = userContext;
    }
}