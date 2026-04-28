using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using visits.api.Auth;
using visits.api.Auth.Services;
using visits.api.Data;
using visits.models.Base;

namespace visits.tests.Common.Factories;

public static class AuthServiceFactory
{
    public static AuthService Create(
        Mock<UserManager<BaseUser>> userManager,
        AppDbContext context,
        JwtSettings? jwtSettings = null)
    {
        var options = Options.Create(jwtSettings ?? new JwtSettings
        {
            Secret = "THIS_IS_A_SUPER_LONG_TEST_SECRET_KEY_123456789",
            Issuer = "test",
            Audience = "test",
            ExpiryMinutes = 60
        });

        return new AuthService(
            userManager.Object,
            context,
            options);
    }
}