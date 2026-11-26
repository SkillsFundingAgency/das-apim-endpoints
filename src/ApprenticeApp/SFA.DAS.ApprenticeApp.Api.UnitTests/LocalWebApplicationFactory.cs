using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApprenticeApp.Telemetry;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using System.Collections.Generic;


namespace SFA.DAS.ApprenticeApp.Api.UnitTests
{
    public class LocalWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        private readonly Dictionary<string, string> _config;
        private readonly TestContext _testContext;


        public LocalWebApplicationFactory(Dictionary<string, string> config, TestContext testContext)
        {
            _config = config;
            _testContext = testContext;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(s =>
            {
                s.Configure<ApprenticeAccountsApiConfiguration>(c =>
                {
                    c.Url = _testContext.ApprenticeAccountsInnerApi.BaseAddress;
                });
                s.Configure<ApprenticeCommitmentsApiConfiguration>(c =>
                {
                    c.Url = _testContext.ApprenticeCommitmentsInnerApi.BaseAddress;
                });

                s.AddMetrics();
                s.AddSingleton<IApprenticeAppMetrics, ApprenticeAppMetrics>();
            });

            builder.ConfigureAppConfiguration(a =>
            {
                a.Sources.Clear();
                a.AddInMemoryCollection(_config);
            });
            builder.UseEnvironment("Development");
        }
    }
}