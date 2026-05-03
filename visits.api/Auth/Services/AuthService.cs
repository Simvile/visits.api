using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using visits.api.Auth.DTOs;
using visits.api.Data;
using visits.api.Utils;
using visits.models.Base;
using visits.models.Core;
using visits.models.Users;

namespace visits.api.Auth.Services;

public class AuthService( UserManager<BaseUser> userManager, AppDbContext context, IOptions<JwtSettings> jwtSettings) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    #region Create New User
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // let's check if there's any existing User with the same email
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new InvalidOperationException("A user with this email already exists.");

        // let's create the base or core user first
        var user = new BaseUser
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            InstitutionId = request.Institution.Id,
            UserTypeId = request.UserType.Id,
            RoleId = request.Role.Id,
            IsActive = true,
            CreatedBy = request.FullName,
            CreatedAt = DateTime.UtcNow,
            UpdatedBy = request.FullName,
            UpdatedAt = DateTime.UtcNow
        };

        // We need to run everything in transaction
        // Define Transaction Scope
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            // Save The Base User
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            // Now We need to create a user account based on the selected UserType. Let's do this using a switch statement
            switch (request.UserType.Description)
            {
                case nameof(UserTypes.Student):
                    var student = new Student
                    {
                        UserId = user.Id,
                        StudentNumber = request.StudentNumber,
                        IsActive = true,
                        CreatedBy = request.FullName,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedBy = request.FullName,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await context.Students.AddAsync(student);
                    break;

                case nameof(UserTypes.Staff):
                    var staff = new Staff
                    {
                        UserId = user.Id,
                        IdentityNumber = request.IdentityNumber
                                         ?? throw new ArgumentNullException(nameof(request.IdentityNumber)),
                        IsActive = true,
                        CreatedBy = request.FullName,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedBy = request.FullName,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await context.Staff.AddAsync(staff);
                    break;

                default:
                    throw new InvalidOperationException("Unknown user type.");
            }

            // Save account and Commit the transaction
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        
        // We need to create token and Sign the user in.
        return await GenerateAuthResponseAsync(user);
    }
    #endregion

    #region Signin
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("This account has been deactivated.");

        return await GenerateAuthResponseAsync(user);
    }
    #endregion

    #region Refresh Token
    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // IsValidate the expired JWT
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Invalid access token.");

        // Find the refresh token
        var refreshToken = await context.RefreshTokens
            .FirstOrDefaultAsync(r =>
                r.Token == request.RefreshToken &&
                r.UserId == Guid.Parse(userId) &&
                !r.IsUsed &&
                !r.IsRevoked)
            ?? throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        if (refreshToken.ExpiryAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token has expired.");

        // Revoke old refresh token
        refreshToken.IsUsed = true;
        context.RefreshTokens.Update(refreshToken);
        await context.SaveChangesAsync();

        // Issue new tokens
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new UnauthorizedAccessException("User not found.");

        return await GenerateAuthResponseAsync(user);
    }
    #endregion

    #region Private Methods
    private async Task<AuthResponse> GenerateAuthResponseAsync(BaseUser user)
    {
        var (accessToken, jwtId, accessTokenExpiry) = await GenerateJwtToken(user);
        var (refreshToken, refreshTokenExpiry) = await GenerateRefreshTokenAsync(user, jwtId);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiry = accessTokenExpiry,
            RefreshTokenExpiry = refreshTokenExpiry,
            User = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                InstitutionId = user.InstitutionId
            }
        };
    }

    private async Task<(string token, string jwtId, DateTime expiry)> GenerateJwtToken(BaseUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
        var jwtId = Guid.NewGuid().ToString();
        
        var roles = await userManager.GetRolesAsync(user);
        var claimsWithRoles = roles.Select(x => new Claim(ClaimTypes.Role, x));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim("user_id", user.Id.ToString()),
            new Claim("tenant_id", user.InstitutionId.ToString()),
            new Claim("fullName", user.FullName),
            new Claim("institutionId", user.InstitutionId.ToString())
        }.Union(claimsWithRoles);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiry,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), jwtId, expiry);
    }

    private async Task<(string token, DateTime expiry)> GenerateRefreshTokenAsync(BaseUser user, string jwtId)
    {
        // Revoke any existing active refresh tokens for this user
        var existingTokens = await context.RefreshTokens
            .Where(r => r.UserId == user.Id && !r.IsUsed && !r.IsRevoked)
            .ToListAsync();

        foreach (var token in existingTokens)
            token.IsRevoked = true;

        var expiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            JwtId = jwtId,
            IsUsed = false,
            IsRevoked = false,
            ExpiryAt = expiry,
            CreatedBy = user.Email!,
            CreatedAt = DateTime.UtcNow,
            UpdatedBy = user.Email!,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();

        return (refreshToken.Token, expiry);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = false // allow expired tokens for refresh
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, parameters, out var validatedToken);

        if (validatedToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            throw new UnauthorizedAccessException("Invalid token.");

        return principal;
    }
    #endregion
}