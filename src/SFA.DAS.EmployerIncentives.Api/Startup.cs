using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SFA.DAS.EmployerIncentives.Api.AppStart;
using SFA.DAS.EmployerIncentives.Api.Authentication;
using SFA.DAS.EmployerIncentives.Api.Authorization;
using SFA.DAS.EmployerIncentives.Api.Configuration;
using SFA.DAS.EmployerIncentives.Api.ErrorHandler;
using SFA.DAS.EmployerIncentives.Api.HealthChecks;

namespace SFA.DAS.EmployerIncentives.Api
{
    public class Startup
    {

        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            _configuration = configuration;

            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration);

            if (_env.IsDevelopment())
            {
                config.AddJsonFile($"appsettings.development.json", optional: true);
            }

            if (_env.IsEnvironment("AcceptanceTests"))
            {
                config.AddJsonFile($"appsettings.acceptancetests.json", optional: false);
            }

            _configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfigurationSections(_configuration);
            services.AddApiAuthentication(_configuration);
            services.AddApiAuthorization(_env);
            
            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
            services.AddMvc(o => o.Filters.Add(new AuthorizeFilter("default")))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDasHealthChecks();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployerIncentivesApi", Version = "v1" });
            });

            services.AddDasHttpClientsAndAssociatedServices(_configuration, _env);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection()
                .UseApiGlobalExceptionHandler(loggerFactory.CreateLogger("Startup"))
                .UseDasHealthChecks()
                .UseAuthentication()
                .UseMvc();

            app.UseSwagger()
                .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployerIncentivesApi");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
