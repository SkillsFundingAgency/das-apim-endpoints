using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerAccounts.Services
{
    public static class DateTimeService
    {
        public static IServiceCollection AddDateTimeServices(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudCurrentTime = configuration.GetValue<string>("EmployerAccountsOuterOverrideDatetime");

            if (!DateTime.TryParse(cloudCurrentTime, out var currentDateTime))
            {
                currentDateTime = DateTime.UtcNow;
            }

            services.AddSingleton<ICurrentDateTime>(_ => new CurrentDateTime(currentDateTime));

            return services;
        }
    }

    public sealed class CurrentDateTime : ICurrentDateTime
    {
        public DateTime Now { get; }

        public CurrentDateTime()
        {
            Now = DateTime.UtcNow;
        }

        public CurrentDateTime(DateTime time)
        {
            Now = time;
        }
    }

    public interface ICurrentDateTime
    {
        DateTime Now { get; }
    }
}
