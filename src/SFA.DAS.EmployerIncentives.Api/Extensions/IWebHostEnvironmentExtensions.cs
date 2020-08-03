using Microsoft.Extensions.Hosting;

namespace SFA.DAS.EmployerIncentives.Api.Extensions
{
    public static class IWebHostEnvironmentExtensions
    {
        public static bool IsLocalOrDev(this Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            return environment.IsDevelopment() || environment.IsLocal();
        }
        public static bool IsLocal(this Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            return environment.IsEnvironment("LOCAL");
        }
    }
}
