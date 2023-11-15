using ASP.NET_CORE7_API_OAUTH2_RESOURCE.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NET_CORE7_API_OAUTH2_RESOURCE.Policies.Handlers {
    public class RequireScopeHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement) {
            // The scope must have originated from our issuer.
            var scopeClaim = context.User.FindAll(c => c.Type == "scope" && c.Issuer == requirement.Issuer);
            if (scopeClaim == null || !scopeClaim.Any())
                return Task.CompletedTask;

            // A token can contain multiple scopes and we need at least one exact match.
            if (scopeClaim.Any(s => s.Value == requirement.Scope))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}