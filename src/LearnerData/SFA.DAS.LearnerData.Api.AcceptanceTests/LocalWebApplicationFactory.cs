using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests;

public class LocalWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    private readonly Dictionary<string, string> _config;

    public LocalWebApplicationFactory(Dictionary<string, string> config)
    {
        _config = config;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Console.WriteLine("Configuring web host...");

        builder.ConfigureAppConfiguration(a =>
        {
            a.AddInMemoryCollection(_config);
        });
        builder.UseEnvironment("LOCAL_ACCEPTANCE_TESTS");
        builder.ConfigureServices(services =>
        {
            var nsbDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IMessageSession));
            if (nsbDescriptor != null) services.Remove(nsbDescriptor);
            services.AddSingleton<IMessageSession>(new StubMessageSession());
        });
    }
}
