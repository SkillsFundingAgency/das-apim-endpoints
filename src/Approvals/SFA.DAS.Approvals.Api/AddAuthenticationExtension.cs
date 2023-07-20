using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;

namespace SFA.DAS.Approvals.Api
{
    public static class AddAuthenticationExtension
    {
        public static void AddAuthentication2(this IServiceCollection services, AzureActiveDirectoryConfiguration config, Dictionary<string, string> policies)
        {
            services.AddAuthorization(delegate (Microsoft.AspNetCore.Authorization.AuthorizationOptions o)
            {
                foreach (KeyValuePair<string, string> policyName in policies)
                {
                    o.AddPolicy(policyName.Key, delegate (Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder policy)
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(policyName.Value);
                    });
                }
            });
            services.AddAuthentication(delegate (Microsoft.AspNetCore.Authentication.AuthenticationOptions auth)
            {
                auth.DefaultScheme = "Bearer";
            }).AddJwtBearer(delegate (Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerOptions auth)
            {
                auth.Authority = "https://login.microsoftonline.com/" + config.Tenant;
                auth.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = config.Identifier.Split(",")
                };
            });
            services.AddSingleton<IClaimsTransformation, AzureAdScopeClaimTransformation2>();
        }
    }

    public class AzureAdScopeClaimTransformation2 : IClaimsTransformation
    {
        private readonly ILogger<AzureAdScopeClaimTransformation2> _logger;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        public AzureAdScopeClaimTransformation2(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor
            , ILogger<AzureAdScopeClaimTransformation2> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            _logger.LogInformation("In TransformAsync");
            AddProviderOrEmployerClaim(principal);
            List<Claim> list = principal.FindAll("http://schemas.microsoft.com/identity/claims/scope").ToList();
            if (list.Count != 1 || !list[0].Value.Contains(' '))
            {
                return Task.FromResult(principal);
            }

            IEnumerable<Claim> claims = from s in list[0].Value.Split(' ')
                                        select new Claim("http://schemas.microsoft.com/identity/claims/scope", s);
            return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(principal.Identity, claims)));
        }

        private void AddProviderOrEmployerClaim(ClaimsPrincipal principal)
        {
            var userParty = _httpContextAccessor.HttpContext.Request.Headers["x-party"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(userParty) && (userParty.ToLower() == "provider" || userParty.ToLower() == "employer"))
            {
                // Remove existing claim for Role of Provider or Employer
                var claimsIdentity = principal.Identity as ClaimsIdentity;
                var claimsToRemove = claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role && (x.Value.ToLower() == "provider" || x.Value.ToLower() == "employer"));
                foreach (var c in claimsToRemove.ToList())
                {
                    claimsIdentity.RemoveClaim(c);
                }

                // Add a new claim with provider or employer for role
                var userPartyEmployerOrProvider = userParty.ToLower() == "provider" ? "Provider" : "Employer";
                var userPartyClaim = new Claim(ClaimTypes.Role, userPartyEmployerOrProvider);
                claimsIdentity.AddClaim(userPartyClaim);
            }
        }
    }
}