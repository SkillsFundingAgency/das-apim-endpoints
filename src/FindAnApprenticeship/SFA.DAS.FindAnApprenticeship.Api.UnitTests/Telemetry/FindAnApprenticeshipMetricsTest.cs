using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics.Testing;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.FindAnApprenticeship.Telemetry;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Diagnostics.Metrics;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Telemetry
{
    [TestFixture]
    public class FindAnApprenticeshipMetricsTest
    {
        [Test, MoqAutoData]
        public void WhenVacancyView_Increased_ThenTheTotalAmountOfVacancyVisit_RecordedSuccessfully(string vacancyReference, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, Constants.OpenTelemetry.ServiceMeterName, Constants.OpenTelemetry.VacancySearchViewsCounterName);

            // Act
            metrics.IncreaseVacancyViews(vacancyReference, viewCount);

            // Assert
            var measurements = collector.GetMeasurementSnapshot();
            measurements.EvaluateAsCounter().Should().Be(viewCount);
        }

        [Test, MoqAutoData]
        public void WhenVacancyStarted_Increased_ThenTheTotalAmountOfVacancyStarted_RecordedSuccessfully(string vacancyReference, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, Constants.OpenTelemetry.ServiceMeterName, Constants.OpenTelemetry.VacancyStartedCounterName);

            // Act
            metrics.IncreaseVacancyStarted(vacancyReference, viewCount);

            // Assert
            var measurements = collector.GetMeasurementSnapshot();
            measurements.EvaluateAsCounter().Should().Be(viewCount);
        }

        [Test, MoqAutoData]
        public void WhenVacancySubmitted_Increased_ThenTheTotalAmountOfVacancySubmitted_RecordedSuccessfully(string vacancyReference, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, Constants.OpenTelemetry.ServiceMeterName, Constants.OpenTelemetry.VacancySubmittedCounterName);

            // Act
            metrics.IncreaseVacancySubmitted(vacancyReference, viewCount);

            // Assert
            var measurements = collector.GetMeasurementSnapshot();
            measurements.EvaluateAsCounter().Should().Be(viewCount);
        }

        [Test, MoqAutoData]
        public void WhenVacancy_Appeared_In_SearchResults_Increased_ThenTheTotalAmountOfVacancySearch_RecordedSuccessfully(string vacancyReference, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, Constants.OpenTelemetry.ServiceMeterName, Constants.OpenTelemetry.VacancySearchResultCounterName);

            // Act
            metrics.IncreaseVacancySearchResultViews(vacancyReference, viewCount);

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
            serviceCollection.AddSingleton<IMetrics, FindAnApprenticeshipMetrics>();
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
