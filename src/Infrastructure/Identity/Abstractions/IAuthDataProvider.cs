using System.Security.Claims;

namespace Infrastructure.Identity.Abstractions
{
    public interface IAuthDataProvider : IClaimType
    {
        string NewId();
        IEnumerable<Claim> GetAllClaims { get; }
        IEnumerable<string> MasterUsers { get; }
    }
}