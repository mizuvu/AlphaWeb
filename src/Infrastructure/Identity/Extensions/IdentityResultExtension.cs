using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Extensions;

public static class IdentityResultExtensions
{
    public static Result ToResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description).ToList());
    }

    public static Result<T> ToResult<T>(this IdentityResult result, T data)
    {
        return result.Succeeded
            ? Result<T>.Success(data: data)
            : Result<T>.Failure(result.Errors.Select(e => e.Description).ToList());
    }
}
