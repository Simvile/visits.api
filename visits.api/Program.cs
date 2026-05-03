using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using visits.api.Auth;
using visits.api.Auth.Services;
using visits.api.Configurations;
using visits.api.Data;
using visits.api.Data.Seeders;
using visits.api.Interfaces;
using visits.api.Manager;
using visits.api.Services;
using visits.models.Base;


var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings));

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<BaseUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Service Configurations
builder.Services.Configure<JwtSettings>(jwtSettings);

// JWT Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // no grace period on expiry
        };
    });

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole(AppRoles.SuperAdmin));

    options.AddPolicy("AdminAndAbove", policy =>
        policy.RequireRole(AppRoles.SuperAdmin, AppRoles.Admin));

    options.AddPolicy("WardenAndAbove", policy =>
        policy.RequireRole(AppRoles.SuperAdmin, AppRoles.Admin, AppRoles.Warden));
});

// Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContextService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInstitutionService, InstitutionService>();
builder.Services.AddScoped<IClassificationValueService, ClassificationValueService>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddManagerConfigurations();
builder.Services.AddAuthorization();

var app = builder.Build();

// Auto-create DB and run migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    await db.Database.MigrateAsync();
    await RoleSeeder.SeedAsync(roleManager, logger);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Visits API";
        options.Theme = ScalarTheme.BluePlanet;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();