using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Admin.Api.Infrastructure.HealthCheck;
using SFA.DAS.DigitalCertificates.Contracts.Client;

namespace SFA.DAS.Admin.Api.UnitTests.HealthCheck;

[TestFixture]
public class DigitalCertificatesApiHealthCheckTests
{
    [Test]
    public void AddCheck_registers_health_check_in_options()
    {
        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>>());

        services.AddHealthChecks()
            .AddCheck<DigitalCertificatesApiHealthCheck>(DigitalCertificatesApiHealthCheck.HealthCheckResultDescription);

        var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<HealthCheckServiceOptions>>().Value;

        Assert.IsTrue(options.Registrations.Any(r => r.Name == DigitalCertificatesApiHealthCheck.HealthCheckResultDescription));
    }
}
