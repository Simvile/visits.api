using FluentAssertions;
using visits.tests.Common.Builders;
using visits.tests.Common.Factories;
using visits.tests.Common.Fixtures;
using visits.tests.Common.Mocks;

namespace visits.tests.Integration;

public class AuthFlowTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public AuthFlowTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task RegisterAsync_Should_Create_Student_And_Return_Token()
    {
        // Arrange
        var userManager = UserManagerMock.CreateSuccess();

        var service = AuthServiceFactory.Create(
            userManager,
            _fixture.Context);

        var request = new RegisterRequestBuilder()
            .WithStudent("ST123")
            .WithEmail("student@test.com")
            .Build();

        // Act
        var result = await service.RegisterAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();

        var student = _fixture.Context.Students.SingleOrDefault();

        student.Should().NotBeNull();
        student.StudentNumber.Should().Be("ST123");
    }
}