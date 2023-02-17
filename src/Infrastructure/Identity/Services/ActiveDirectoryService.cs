using Application.Identity;
using Infrastructure.Identity.Models;
using Microsoft.Extensions.Options;
using Shared.Identity;
using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;

namespace Infrastructure.Identity.Services;

[SupportedOSPlatform("windows")]
public class ActiveDirectoryService : IActiveDirectoryService
{
    private readonly string _domain;

    public ActiveDirectoryService(IOptions<ActiveDirectory> settings)
    {
        _domain = settings.Value.Domain;
    }

    public Task<bool> CheckPasswordSignInAsync(string userName, string password)
    {
        // Create a context that will allow you to connect to your Domain Controller
        using (var adContext = new PrincipalContext(ContextType.Domain, _domain))
        {
            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(adContext, userName);
            if (user is not null && !user.IsAccountLockedOut())
            {
                //Check user is blocked
                var result = adContext.ValidateCredentials(userName, password);
                return Task.FromResult(result);
            }
        };

        return Task.FromResult(false);
    }

    public Task<UserDto> GetByUserNameAsync(string userName)
    {
        using var adContext = new PrincipalContext(ContextType.Domain, _domain);
        {
            var adUser = UserPrincipal.FindByIdentity(adContext, userName);
            _ = adUser ?? throw new NotFoundException(userName, nameof(adUser));

            var result = new UserDto
            {
                FirstName = adUser.GivenName,
                LastName = adUser.Surname,
                PhoneNumber = adUser.VoiceTelephoneNumber,
                Email = adUser.EmailAddress,
            };

            return Task.FromResult(result);
        }
    }
}
