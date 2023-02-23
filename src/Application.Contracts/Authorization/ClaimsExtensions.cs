using System.Reflection;
using System.Security.Claims;

namespace Application.Contracts.Authorization
{
    public class ClaimsExtensions
    {
        private static IEnumerable<Claim>? _allClaims;

        public static IEnumerable<Claim> AppClaims
        {
            get
            {
                _allClaims ??= GetAppClaims();
                return _allClaims;
            }
            set { _allClaims = value; }
        }

        private static IEnumerable<Claim> GetAppClaims()
        {
            var fromClass = typeof(Permissions);

            var claims = new List<Claim>();

            // get classes in class
            var modules = fromClass.GetNestedTypes();

            foreach (var module in modules)
            {
                // get props in class
                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (FieldInfo fi in fields)
                {
                    var propertyValue = fi.GetValue(null);

                    if (propertyValue != null)
                    {
                        claims.Add(new Claim(AppClaimType.Permission, propertyValue.ToString() ?? string.Empty));
                    }
                    //TODO - take descriptions from description attribute
                }
            }

            return claims;
        }
    }
}