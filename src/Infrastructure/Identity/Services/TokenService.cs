using Application.Identity;
using Infrastructure.Auth.Jwt;
using Infrastructure.Identity.Abstractions;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Identity;
using System.Security.Claims;

namespace Infrastructure.Identity.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly JwtConfiguration _jwtSettings;
    private readonly IActiveDirectoryService _adService;
    private readonly IIdentityDbContext _db;
    private readonly IAuthDataProvider _authDefinition;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<JwtConfiguration> jwtSettings,
        IActiveDirectoryService userADService,
        IIdentityDbContext db,
        IAuthDataProvider authDefinition)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
        _adService = userADService;
        _db = db;
        _authDefinition = authDefinition;
    }

    public async Task<TokenDto> GetTokenAsync(TokenRequest request)
    {
        var userName = request.UserName;

        // check login
        bool loginSuccess;

        var user = await _userManager.FindByNameAsync(userName);
        _ = user ?? throw new UnauthorizedException("Invalid credentials.");

        if (user.IsBlocked)
            throw new UnauthorizedException("User is blocked.");

        if (user.UseDomainPassword) // login by domain password
            loginSuccess = await _adService.CheckPasswordSignInAsync(request.UserName, request.Password);
        else // login by local password
            loginSuccess = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!loginSuccess)
            throw new UnauthorizedException("Invalid credentials.");

        var token = await GenerateJwtAsync(user);
        var tokenExpiryTime = _jwtSettings.TokenExpireInSeconds;
        var refreshToken = JwtHelper.GenerateRefreshToken();
        var refreshTokenExpiryTime = _jwtSettings.RefreshTokenExpireInSeconds;

        await SaveTokenAsync(
            user.Id,
            token, DateTimeOffset.Now.AddSeconds(tokenExpiryTime),
            refreshToken, DateTimeOffset.Now.AddSeconds(refreshTokenExpiryTime));

        await _userManager.UpdateAsync(user);

        return new TokenDto
        {
            AccessToken = token,
            TokenExpiryTime = _jwtSettings.TokenExpireInSeconds,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime,
        };
    }

    public async Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userPrincipal = JwtHelper.GetPrincipalFromExpiredToken(
            request.AccessToken,
            _jwtSettings.SecretKey,
            _jwtSettings.TokenIssuer);

        var userId = userPrincipal.FindFirstValue(_authDefinition.UserId);
        _ = userId ?? throw new UnauthorizedException("Invalid credentials.");

        var user = await _userManager.FindByIdAsync(userId);
        _ = user ?? throw new UnauthorizedException("Invalid credentials.");

        var userToken = await GetTokenAsync(userId);
        if (userToken is null
            || userToken.RefreshToken != request.RefreshToken
            || userToken.RefreshTokenExpiryTime <= DateTime.Now)
            throw new UnauthorizedException("Refresh token invalid.");

        var token = await GenerateJwtAsync(user);
        var tokenExpiryTime = _jwtSettings.TokenExpireInSeconds;
        var refreshToken = JwtHelper.GenerateRefreshToken();
        var refreshTokenExpiryTime = _jwtSettings.RefreshTokenExpireInSeconds;

        // update user login info
        await SaveTokenAsync(
            user.Id,
            token, DateTimeOffset.Now.AddSeconds(tokenExpiryTime),
            refreshToken, DateTimeOffset.Now.AddSeconds(refreshTokenExpiryTime));

        await _userManager.UpdateAsync(user);

        return new TokenDto
        {
            AccessToken = token,
            TokenExpiryTime = _jwtSettings.TokenExpireInSeconds,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime,
        };
    }

    private async Task<string> GenerateJwtAsync(ApplicationUser user)
    {
        var signingCredentials = JwtHelper.GetSigningCredentials(_jwtSettings.SecretKey);

        var token = JwtHelper.GenerateEncryptedToken(
            signingCredentials,
            await GetClaimsAsync(user),
            _jwtSettings.TokenIssuer,
            _jwtSettings.TokenExpireInSeconds);

        return token;
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var thisRole = await _roleManager.FindByNameAsync(role);
            if (thisRole is null)
                continue;

            var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
            permissionClaims.AddRange(allPermissionsForThisRoles);
        }

        var claims = new List<Claim>
        {
            new(_authDefinition.UserId, user.Id ?? string.Empty),
            new(_authDefinition.UserName, user.UserName ?? string.Empty),
            new(_authDefinition.FirstName, user.FirstName ?? string.Empty),
            new(_authDefinition.LastName, user.LastName ?? string.Empty),
            new(_authDefinition.PhoneNumber, user.PhoneNumber ?? string.Empty),
            new(_authDefinition.Email, user.Email ?? string.Empty),
        }
        .Union(userClaims)
        .Union(roleClaims)
        .Union(permissionClaims);

        return claims;
    }

    private async Task SaveTokenAsync(string userId,
        string token, DateTimeOffset tokenExpiryTime,
        string refreshToken, DateTimeOffset refreshTokenExpiryTime)
    {
        var entry = await _db.JwtTokens.FindAsync(userId);

        if (entry is not null)
        {
            entry.Token = token;
            entry.TokenExpiryTime = tokenExpiryTime;
            entry.RefreshToken = refreshToken;
            entry.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _db.SaveChangesAsync();
        }
        else
        {
            var entity = new JwtToken
            {
                UserId = userId,
                Token = token,
                TokenExpiryTime = tokenExpiryTime,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiryTime,
            };

            await _db.JwtTokens.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
    }

    private async Task<JwtToken?> GetTokenAsync(string userId)
    {
        return await _db.JwtTokens.FindAsync(userId);
    }
}
