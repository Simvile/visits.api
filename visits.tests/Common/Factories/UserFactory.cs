using visits.models.Base;

namespace visits.tests.Common.Factories;

public static class UserFactory
{
    public static BaseUser Create()
    {
        return new BaseUser
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            FullName = "Test User",
            
            CreatedAt = DateTime.Now,
            CreatedBy = "Test",
            UpdatedAt = DateTime.Now,
            UpdatedBy = "Test",
        };
    }
}