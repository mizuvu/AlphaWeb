using Shared.Identity;

namespace Application.Identity;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(bool isMasterUser = false, CancellationToken cancellationToken = default);
    Task<PagedList<UserDto>> FindAsync(UserLookup request, bool isMasterUser = false, CancellationToken cancellationToken = default);
    Task<UserDto> GetByIdAsync(string id);
    Task<UserDto> GetByUserNameAsync(string userName);
    Task<Result<string>> CreateAsync(UserDto newUser);
    Task<Result> UpdateAsync(UserDto updateUser);
    Task<Result> DeleteAsync(string id);
    Task<Result> ForcePasswordAsync(string id, string password);
}