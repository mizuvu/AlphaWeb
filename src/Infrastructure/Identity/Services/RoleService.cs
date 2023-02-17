using Application.Identity;
using Infrastructure.Identity.Abstractions;
using Infrastructure.Identity.Extensions;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Identity;
using System.Security.Claims;

namespace Infrastructure.Identity.Services;

public class RoleService : IRoleService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IIdentityDbContext _db;
    private readonly IAuthDataProvider _authDefinition;

    public RoleService(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IIdentityDbContext db,
        IAuthDataProvider authDefinition)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
        _authDefinition = authDefinition;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _roleManager.Roles
            .AsNoTracking()
            .ProjectToType<RoleDto>()
            .ToListAsync(cancellationToken);
    }

    public async Task<RoleDto> GetByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        _ = role ?? throw new NotFoundException(id, nameof(role));

        var result = role.Adapt<RoleDto>();

        var claims = await _roleManager.GetClaimsAsync(role);
        result.Claims = claims.Select(s => s.Value);

        return result;
    }

    public async Task<Result> CreateAsync(RoleDto request)
    {
        var role = new ApplicationRole
        {
            Id = _authDefinition.NewId(),
            Name = request.Name.Trim().ToLower(),
            Description = request.Description,
        };

        var result = await _roleManager.CreateAsync(role);

        return result.ToResult();
    }

    public async Task<Result> UpdateAsync(RoleDto request)
    {
        var role = await _roleManager.FindByIdAsync(request.Id);
        _ = role ?? throw new NotFoundException(request.Id, nameof(role));

        role.Name = request.Name.Trim().ToLower();
        role.Description = request.Description;

        var result = await _roleManager.UpdateAsync(role);

        //get claims by role
        var roleClaims = await _roleManager.GetClaimsAsync(role);

        // remove claims not in request list
        var claimsToRemove = roleClaims.Where(x => !request.Claims.Contains(x.Value)).AsEnumerable();
        foreach (var claim in claimsToRemove)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        foreach (var claim in request.Claims)
        {
            // check claim in Claims Store
            var isClaimAvailable = _authDefinition.GetAllClaims
                .Select(x => x.Value)
                .Contains(claim);

            // check role already owned claim
            var stillNotHaveThisClaim = !roleClaims.Select(s => s.Value).Contains(claim);

            if (!isClaimAvailable || !stillNotHaveThisClaim) continue;

            var newClaim = new Claim(_authDefinition.Permission, claim);
            await _roleManager.AddClaimAsync(role, newClaim);
        }

        return result.ToResult();
    }

    public async Task<Result> DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        _ = role ?? throw new NotFoundException(id, nameof(role));

        //Check claim exist for role
        var claimsByRole = await _roleManager.GetClaimsAsync(role);

        if (claimsByRole.Any())
            return Result.Failure("Role has already setup claim.");

        var result = await _roleManager.DeleteAsync(role);

        return result.ToResult();
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync(string roleName,
        CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        _ = role ?? throw new NotFoundException(roleName, nameof(role));

        var userIdsInRole = await _db.ApplicationUserRoles
            .Where(x => x.RoleId == role.Id)
            .Select(x => x.UserId)
            .ToListAsync(cancellationToken);

        return await _userManager.Users.Where(x => userIdsInRole.Contains(x.Id))
            .ProjectToType<UserDto>()
            .ToListAsync(cancellationToken);
    }
    /*
    public async Task<IResult> UpdateClaimsAsync(string id, string[] claims)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
            return Result.Failure("Role not found.");

        //get claims by role Id
        var roleClaims = await _roleManager.GetClaimsAsync(role);

        // remove claims not in request list
        var claimsToRemove = roleClaims.Where(x => !claims.Contains(x.Value)).AsEnumerable();
        foreach (var claim in claimsToRemove)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        foreach (var claim in claims)
        {
            // check claim in Claims Store
            var isClaimAvailable = ClaimsExtensions.GetAppClaims()
                .Select(x => x.Value)
                .Contains(claim);

            // check role already owned claim
            var stillNotHaveThisClaim = !roleClaims.Select(s => s.Value).Contains(claim);

            if (!isClaimAvailable || !stillNotHaveThisClaim) continue;

            var newClaim = new Claim(AppClaims.Permission, claim);
            await _roleManager.AddClaimAsync(role, newClaim);
        }

        return Result.Success();
    }
    */
}
