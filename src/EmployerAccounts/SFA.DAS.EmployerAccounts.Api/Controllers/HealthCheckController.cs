using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.EmployerAccounts.Application.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerAccounts.Api.Controllers
{
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private const string HealthCheckResultDescription = "Employer Accounts Outer Api Health Check";
        private readonly IEmployerAccountsService _employerAccountsService;

        public HealthCheckController(IEmployerAccountsService employerAccountsService)
        {
            _employerAccountsService = employerAccountsService;
        }

        [HttpGet]
        [Route("healthcheck")]        
        public async Task<HealthCheckResult> CheckHealth()
        {
            var timer = Stopwatch.StartNew();
            var isHealthy = await _employerAccountsService.IsHealthy();
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            return (isHealthy ? HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object> { { "Duration", durationString } })
                : HealthCheckResult.Unhealthy(HealthCheckResultDescription, null, new Dictionary<string, object> { { "Duration", durationString } }));
        }       
    }
}
