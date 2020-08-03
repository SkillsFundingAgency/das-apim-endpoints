using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerIncentives.Api.HealthChecks
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDasHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<InnerApiHealthCheck>("Inner Employer Incentives API Health Check")
                .AddCheck<CommitmentsApiHealthCheck>("Commitments V2 API Health Check");

            return services;
        }
    }
}
