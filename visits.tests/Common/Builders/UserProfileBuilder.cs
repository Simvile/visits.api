using visits.api.DTOs;

namespace visits.tests.Common.Builders;

public class UserProfileBuilder
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly string _fullName = "John";
    private readonly string _email = "example@email.com";
    private readonly string _phoneNumber = "0712345678";
    private readonly string _studentNumber = "123STU";

    public UserProfile Build()
    {
        return new UserProfile()
        {
            Id = _id,
            Fullname = _fullName,
            Email = _email,
            PhoneNumber = _phoneNumber,
            StudentNumber = _studentNumber
        };
    }
}