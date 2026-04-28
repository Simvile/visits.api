using Microsoft.AspNetCore.Identity;

namespace visits.api.Data.Seeders;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager, ILogger logger)
    {
        foreach (var role in AppRoles.All)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                logger.LogInformation("Role '{Role}' already exists, skipping.", role);
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = role,
                NormalizedName = role.ToUpperInvariant()
            });

            if (result.Succeeded)
                logger.LogInformation("Role '{Role}' created successfully.", role);
            else
                logger.LogError("Failed to create role '{Role}': {Errors}", role,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

public static class AppRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Warden = "Warden";
    public const string Student = "Student";
    public const string Security = "Security";

    public static IEnumerable<string> All => new[]
    {
        SuperAdmin,
        Admin,
        Warden,
        Student,
        Security
    };
}