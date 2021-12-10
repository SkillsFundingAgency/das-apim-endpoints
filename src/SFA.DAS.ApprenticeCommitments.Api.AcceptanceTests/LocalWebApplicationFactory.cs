using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
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
                s.Configure<ApprenticeLoginConfiguration>(c =>
                {
                    c.Url = _testContext.LoginConfig.Url;
                    c.IdentityServerClientId = _testContext.LoginConfig.IdentityServerClientId;
                    c.CallbackUrl = _testContext.LoginConfig.CallbackUrl;
                    c.RedirectUrl = _testContext.LoginConfig.RedirectUrl;
                });
                s.Configure<ApprenticeAccountsConfiguration>(c =>
                {
                    c.Url = _testContext.InnerApi.BaseAddress;
                });
                s.Configure<CommitmentsV2Configuration>(c =>
                {
                    c.Url = _testContext.CommitmentsV2InnerApi.BaseAddress;
                });
                s.Configure<TrainingProviderConfiguration>(c =>
                {
                    c.Url = _testContext.TrainingProviderInnerApi.BaseAddress;
                });
                s.Configure<CoursesApiConfiguration>(c =>
                {
                    c.Url = _testContext.CoursesInnerApi.BaseAddress;
                });
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