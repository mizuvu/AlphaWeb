using System.Security.Claims;

namespace Web.Services
{
    public interface IAppAuthorizationService
    {
        Task<bool> HasPolicy(ClaimsPrincipal user, string policy);
    }

    public class AppAuthorizationService : IAppAuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;

        public AppAuthorizationService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<bool> HasPolicy(ClaimsPrincipal user, string policy)
        {
            var auth = await _authorizationService.AuthorizeAsync(user, policy);

            return auth.Succeeded;
        }
    }
}
