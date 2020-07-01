using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerIncentives.Api.Configuration;
using SFA.DAS.EmployerIncentives.Configuration;

namespace SFA.DAS.EmployerIncentives.Api.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var temp = config.GetSection(EmployerIncentivesConfigurationKeys.EmployerIncentivesOuterApi).Value;
            var azureActiveDirectoryConfiguration = config.GetSection(EmployerIncentivesConfigurationKeys.AzureActiveDirectoryApiConfiguration).Get<AzureActiveDirectoryApiConfiguration>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(auth =>
            {
                auth.Authority = $"https://login.microsoftonline.com/{azureActiveDirectoryConfiguration.Tenant}";
                auth.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudiences = new List<string>
                    {
                        azureActiveDirectoryConfiguration.Identifier
                    }
                };
            });

            services.AddSingleton<IClaimsTransformation, AzureAdScopeClaimTransformation>();
            return services;
        }
    }

}
