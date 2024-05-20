using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics.Testing;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Api.Telemetry;
using System;
using System.Diagnostics.Metrics;

namespace SFA.DAS.Vacancies.Api.UnitTests.Telemetry
{
    [TestFixture]
    public class VacancyMetricsTest
    {
        [Test, MoqAutoData]
        public void WhenVacancyView_Increased_ThenTheTotalAmountOfVacancyVisit_RecordedSuccessfully(string vacancyReference, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<VacancyMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, Constants.OpenTelemetry.ServiceMeterName, Constants.OpenTelemetry.VacancySearchViewsCounterName);

            // Act
            metrics.IncreaseVacancyViews(vacancyReference, viewCount);

            // Assert
            var measurements = collector.GetMeasurementSnapshot();
            measurements.EvaluateAsCounter().Should().Be(viewCount);
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var config = CreateIConfiguration();
            serviceCollection.AddMetrics();
            serviceCollection.AddSingleton(config);
            serviceCollection.AddSingleton<VacancyMetrics>();
            return serviceCollection.BuildServiceProvider();
        }

        private static IConfiguration CreateIConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();
        }
    }
}
