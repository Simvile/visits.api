using visits.api.Auth.DTOs;
using visits.api.Utils;

namespace visits.tests.Common.Builders;

public class RegisterRequestBuilder
{
    private Guid _institutionId = Guid.NewGuid();
    private Guid _userTypeId = Guid.NewGuid();
    private Guid _roleId = Guid.NewGuid();

    private string _email = "test@test.com";
    private string _tudentNumber = "123123";
    private string _fullName = "Test User";
    private string _password = "Password123!";

    public RegisterRequestBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }
    public RegisterRequestBuilder WithStudent(string number)
    {
        _tudentNumber = number;
        return this;
    }

    public RegisterRequestBuilder WithUserType(Guid userTypeId)
    {
        _userTypeId = userTypeId;
        return this;
    }

    public RegisterRequest Build()
    {
        return new RegisterRequest
        {
            Email = _email,
            FullName = _fullName,
            StudentNumber = _tudentNumber,
            Password = _password,
            Institution = new DropdownModel
            {
                Id = _institutionId
            },
            UserType = new DropdownModel
            {
                Id = _userTypeId,
                Description = "Student"
            },
            Role = new DropdownModel
            {
                Id = _roleId
            }
        };
    }
}