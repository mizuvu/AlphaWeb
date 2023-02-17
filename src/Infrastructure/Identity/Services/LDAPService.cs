using Application.Identity;
using Infrastructure.Identity.Models;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Shared.Identity;
using System.DirectoryServices;
using System.Runtime.Versioning;

namespace Infrastructure.Identity.Services;

[SupportedOSPlatform("windows")]
public class LDAPService : IActiveDirectoryService
{
    private readonly ActiveDirectory _settings;

    public LDAPService(IOptions<ActiveDirectory> settings)
    {
        _settings = settings.Value;
    }

    public Task<bool> CheckPasswordSignInAsync(string userName, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(password.Trim()))
            {
                return Task.FromResult(false);
            }
            // create LDAP connection
            var ldapConn = new LdapConnection() { SecureSocketLayer = false };

            // create socket connect to server
            ldapConn.Connect(_settings.LDAP.IpServer, _settings.LDAP.Port);

            // bind user dm & password
            ldapConn.Bind(userName + _settings.LDAP.DomainUser, password);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public Result ChangePasswordAsync(string userName, string oldPassword, string newPassword)
    {
        var sPath = _settings.LDAP.Connection; //This is if your domain was my.domain.com
        var de = new DirectoryEntry(sPath, _settings.LDAP.UserName, _settings.LDAP.Password, AuthenticationTypes.Secure);
        var ds = new DirectorySearcher(de);
        string qry = string.Format("(&(objectCategory=person)(objectClass=user)(sAMAccountName={0}))", userName);
        ds.Filter = qry;
        try
        {
            var sr = ds.FindOne();
            if (sr is null)
            {
                return Result.Failure("User not found on domain.");
            }

            DirectoryEntry user = sr.GetDirectoryEntry();
            user.Invoke("SetPassword", new object[] { newPassword });
            user.CommitChanges();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public Task<UserDto> GetByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }
}
