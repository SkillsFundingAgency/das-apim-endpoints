using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.Configuration;

namespace SFA.DAS.SharedOuterApi.AppStart
{
    public static class AddAuthenticationExtension
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, AzureActiveDirectoryConfiguration config)
        {
            
            services.AddAuthorization(o =>
            {
                o.AddPolicy("default", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("APIM");
                });
            });
                
            services.AddAuthentication(auth => { auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(auth =>
                {
                    auth.Authority =
                        $"https://login.microsoftonline.com/{config.Tenant}";
                    auth.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidAudiences = new List<string>
                        {
                            config.Identifier
                        }
                    };
                });
            services.AddSingleton<IClaimsTransformation, AzureAdScopeClaimTransformation>();
            return services;
        }
    }
}
