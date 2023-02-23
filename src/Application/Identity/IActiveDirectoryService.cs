using Application.Contracts.Identity;

namespace Application.Identity;

public interface IActiveDirectoryService
{
    /// <summary>
    /// Check userName & password from Active Directory
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<bool> CheckPasswordSignInAsync(string userName, string password);

    /// <summary>
    /// Get User Infomation from Active Directory
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<UserDto> GetByUserNameAsync(string userName);
}