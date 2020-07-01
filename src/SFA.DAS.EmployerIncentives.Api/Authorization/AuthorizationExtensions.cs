using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerIncentives.Api.Authorization
{
    public static class ApiAuthorizationExtensions
    {
        public static IServiceCollection AddApiAuthorization(this IServiceCollection services, IHostingEnvironment environment)
        {
            var isDevelopment = environment.IsDevelopment();

            services.AddAuthorization(o =>
            {
                o.AddPolicy("default", policy =>
                {
                    if (isDevelopment)
                        policy.AllowAnonymousUser();
                    else
                        policy.RequireAuthenticatedUser();
                    policy.RequireRole("Default");
                });
            });
            if (!isDevelopment)
                services.AddSingleton<IAuthorizationHandler, LocalAuthorizationHandler>();
            return services;
        }
    }
}