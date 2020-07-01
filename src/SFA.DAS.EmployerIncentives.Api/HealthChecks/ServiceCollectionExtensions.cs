using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerIncentives.Api.Extensions;

namespace SFA.DAS.EmployerIncentives.Api.HealthChecks
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDasHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<InnerApiHealthCheck>("Inner Employer Incentives API Health Check")
                .AddCheck<CommitmentsV2ApiHealthCheck>("Commitments V2 API Health Check");

            return services;
        }

        public static IApplicationBuilder UseDasHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/ping"); 

            return app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = (c, r) => c.Response.WriteJsonAsync(new
                {
                    r.Status,
                    r.TotalDuration,
                    Results = r.Entries.ToDictionary(
                        e => e.Key,
                        e => new
                        {
                            e.Value.Status,
                            e.Value.Duration,
                            e.Value.Description,
                            e.Value.Data
                        })
                })
            });
        }

    }
}
