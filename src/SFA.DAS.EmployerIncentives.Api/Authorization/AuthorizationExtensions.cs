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
            var isDevelopment = environment.IsDevelopment();

            services.AddAuthorization(o =>
            {
                o.AddPolicy("APIM", policy =>
                {
                    if (isDevelopment)
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
            if (isDevelopment)
                services.AddSingleton<IAuthorizationHandler, LocalAuthorizationHandler>();
            return services;
        }
    }
}