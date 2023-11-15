using Microsoft.AspNetCore.Authorization;

namespace ASP.NET_CORE7_API_OAUTH2_RESOURCE.Policies.Requirements {
    public class ScopeRequirement : IAuthorizationRequirement {
        public string Issuer { get; }

        public string Scope { get; }

        public ScopeRequirement(string issuer, string scope) {
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }
}