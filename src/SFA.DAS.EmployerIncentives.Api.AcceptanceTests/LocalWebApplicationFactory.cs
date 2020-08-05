using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests
{
    public class LocalWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        private readonly Dictionary<string, string> _config;

        public LocalWebApplicationFactory(Dictionary<string, string> config)
        {
            _config = config;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(a =>
            {
                a.AddInMemoryCollection(_config);
            });
            builder.UseEnvironment("Development");
        }
    }
}