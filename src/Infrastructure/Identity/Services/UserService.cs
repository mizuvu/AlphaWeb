using Application.Common.Extensions;
using Application.Contracts.Identity;
using Application.Identity;
using Infrastructure.Identity.Abstractions;
using Infrastructure.Identity.Extensions;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IAuthDataProvider _authDefinition;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IAuthDataProvider authDefinition)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authDefinition = authDefinition;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(bool isMasterUser = false, CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users;

        if (!isMasterUser)
        {
            query = query.Where(x =>
                x.IsDeleted == false
                && !_authDefinition.MasterUsers.Contains(x.UserName!));
        }

        return await query
            .AsNoTracking()
            .OrderBy(x => x.IsDeleted)
            .ThenBy(x => x.UserName)
            .ProjectToType<UserDto>()
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedList<UserDto>> FindAsync(UserLookup request, bool isMasterUser = false, CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users;

        if (!isMasterUser)
        {
            query = query.Where(x =>
                x.IsDeleted == false
                && !_authDefinition.MasterUsers.Contains(x.UserName!));
        }

        if (!string.IsNullOrEmpty(request.Value))
        {
            query = query.Where(x =>
                x.UserName!.Contains(request.Value)
                || x.FirstName.Contains(request.Value)
                || x.LastName.Contains(request.Value));
        }

        var result = await query
            .AsNoTracking()
            .OrderBy(x => x.IsDeleted)
            .ThenBy(x => x.UserName)
            .ProjectToType<UserDto>()
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

        return result;
    }

    private async Task<UserDto> GetUserAsync(ApplicationUser user)
    {
        var result = user.Adapt<UserDto>();

        // check users is master
        if (_authDefinition.MasterUsers.All(a => a != user.UserName))
        {
            // if user is not Master, can view roles has owned
            result.Roles = await _userManager.GetRolesAsync(user);
        }
        else
        {
            var allRoles = await _roleManager.Roles.ToListAsync();
            result.Roles = allRoles.Select(s => s.Name).ToList()!;
        }

        return result;
    }

    public async Task<UserDto> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        _ = user ?? throw new NotFoundException(id, nameof(user));

        return await GetUserAsync(user);
    }

    public async Task<UserDto> GetByUserNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        _ = user ?? throw new NotFoundException(userName, nameof(user));

        return await GetUserAsync(user);
    }

    public async Task<Result<string>> CreateAsync(UserDto newUser)
    {
        var entity = new ApplicationUser
        {
            Id = _authDefinition.NewId(),
            UserName = newUser.UserName,
            Email = newUser.Email,
            PhoneNumber = newUser.PhoneNumber,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            UseDomainPassword = newUser.UseDomainPassword,
        };

        var identityResult = await _userManager.CreateAsync(entity, newUser.Password);

        return identityResult.ToResult(entity.Id);
    }

    public async Task<Result> UpdateAsync(UserDto updateUser)
    {
        var id = updateUser.Id;

        var user = await _userManager.FindByIdAsync(id);
        _ = user ?? throw new NotFoundException(id, nameof(user));

        user.FirstName = updateUser.FirstName;
        user.LastName = updateUser.LastName;
        user.PhoneNumber = updateUser.PhoneNumber;
        user.Email = updateUser.Email;
        user.UseDomainPassword = updateUser.UseDomainPassword;
        user.IsBlocked = updateUser.IsBlocked;

        var updatedResult = await _userManager.UpdateAsync(user);
        if (!updatedResult.Succeeded)
            return updatedResult.ToResult();

        var currentRoles = await _userManager.GetRolesAsync(user);
        var addedRoles = updateUser.Roles.Except(currentRoles);
        var removedRoles = currentRoles.Except(updateUser.Roles);

        if (addedRoles.Any())
        {
            var addRole = await _userManager.AddToRolesAsync(user, addedRoles);
            if (!addRole.Succeeded)
                return addRole.ToResult();
        }

        if (removedRoles.Any())
        {
            var removeRole = await _userManager.RemoveFromRolesAsync(user, removedRoles);
            if (!removeRole.Succeeded)
                return removeRole.ToResult();
        }

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        _ = user ?? throw new NotFoundException(id, nameof(user));

        var identityResult = await _userManager.DeleteAsync(user);

        return identityResult.ToResult();
    }

    public async Task<Result> ForcePasswordAsync(string id, string password)
    {
        var user = await _userManager.FindByIdAsync(id);
        _ = user ?? throw new NotFoundException(id, nameof(user));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var identityResult = await _userManager.ResetPasswordAsync(user, token, password);

        return identityResult.ToResult();
    }
}
