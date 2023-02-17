using Shared.Identity;

namespace Application.Identity;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RoleDto> GetByIdAsync(string id);
    Task<Result> CreateAsync(RoleDto request);
    Task<Result> UpdateAsync(RoleDto request);
    Task<Result> DeleteAsync(string id);
}
