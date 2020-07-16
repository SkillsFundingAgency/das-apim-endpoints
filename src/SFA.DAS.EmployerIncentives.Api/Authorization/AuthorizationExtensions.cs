using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SFA.DAS.EmployerIncentives.Api.Authorization
{
    public static class ApiAuthorizationExtensions
    {
        public static IServiceCollection AddApiAuthorization(this IServiceCollection services, IWebHostEnvironment environment)
        {
            var isLocal = environment.IsDevelopment() || environment.IsEnvironment("LOCAL");

            services.AddAuthorization(o =>
            {
                o.AddPolicy("APIM", policy =>
                {
                    if (isLocal)
                    {
                        policy.AllowAnonymousUser();
                    }
                    else
                    {
                        policy.RequireAuthenticatedUser();
                        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                        policy.RequireRole("APIM");
                    }
                });
            });
            if (isLocal)
                services.AddSingleton<IAuthorizationHandler, LocalAuthorizationHandler>();
            return services;
        }
    }
}