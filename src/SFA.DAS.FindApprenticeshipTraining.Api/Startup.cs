using System;
using System.IO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.FindApprenticeshipTraining.Api.AppStart;
using SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.FindApprenticeshipTraining.Application.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Application.Infrastructure.Api;

namespace SFA.DAS.FindApprenticeshipTraining.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true)
#endif
                .AddEnvironmentVariables();

            if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
                config.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                        options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                        options.EnvironmentName = configuration["Environment"];
                        options.PreFixConfigurationKeys = false;
                    }
                );
            }
            
            _configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton(_env);
            services.Configure<CoursesApiConfiguration>(_configuration.GetSection("CoursesApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(_configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

            var serviceProvider = services.BuildServiceProvider();

            if (!ConfigurationIsLocalOrDev())
            {
                services.AddAuthentication(serviceProvider.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            }

            services.AddMediatR(typeof(GetTrainingCoursesListQuery).Assembly);
            services.AddServiceRegistration();
            
            services
                .AddMvc(o =>
                {
                    if (!ConfigurationIsLocalOrDev())
                    {
                        o.Filters.Add(new AuthorizeFilter("default"));
                    }
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FindApprenticeshipTrainingApi", Version = "v1" });
            });
            
            services.BuildServiceProvider();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();    
        
            
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller=Standards}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FindApprenticeshipTrainingApi");
                c.RoutePrefix = string.Empty;
            });
        }

        private bool ConfigurationIsLocalOrDev()
        {
            return _configuration["Environment"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ||
                   _configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
