using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics.Testing;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Telemetry;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Diagnostics.Metrics;

namespace SFA.DAS.ApprenticeApp.UnitTests.Telemetry
{
    [TestFixture]
    public class TelemetryTests
    {
        [Test, MoqAutoData]
        public void WhenAccountView_Increased_ThenTheTotalAmountOfAccountViews_RecordedSuccessfully(int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IApprenticeAppMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, "MyApprenticeApp", "MyApprentice-App.accounts.views");

            // Act
            metrics.IncreaseAccountViews(viewCount);

            // Assert
            var measurements = collector.GetMeasurementSnapshot();
            measurements.EvaluateAsCounter().Should().Be(viewCount);

        }

        [Test, MoqAutoData]
        public void WhenKSBsView_Increased_ThenTheTotalAmountOfKSBsViews_RecordedSuccessfully(string courseId, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IApprenticeAppMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, "MyApprenticeApp", "MyApprentice-App.ksb.views");

            // Act
            metrics.IncreaseKSBsViews(courseId, viewCount);

            // Assert
            var measurements = collector.GetMeasurementSnapshot();
            measurements.EvaluateAsCounter().Should().Be(viewCount);
        }

        [Test, MoqAutoData]
        public void WhenKSBProgressStarted_Increased_ThenTheTotalAmountOfKSBInProgress_RecordedSuccessfully(string courseId, string ksbId, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IApprenticeAppMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, "MyApprenticeApp", "MyApprentice-App.ksb.inprogress");

            // Act
            metrics.IncreaseKSBInProgress(courseId, ksbId, viewCount);

            // Assert
            var measurements = collector.GetMeasurementSnapshot();
            measurements.EvaluateAsCounter().Should().Be(viewCount);
        }

        [Test, MoqAutoData]
        public void WhenKSBCompleted_Increased_ThenTheTotalAmountOfKSBCompleted_RecordedSuccessfully(string courseId, string ksbId, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IApprenticeAppMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, "MyApprenticeApp", "MyApprentice-App.ksb.completed");

            // Act
            metrics.IncreaseKSBCompleted(courseId, ksbId, viewCount);

            // Assert
            var measurements = collector.GetMeasurementSnapshot();
            measurements.EvaluateAsCounter().Should().Be(viewCount);
        }

        [Test, MoqAutoData]
        public void WhenSupportGuidanceArticleViews_Increased_ThenTheTotalSupportGuidanceArticleView_RecordedSuccessfully(string articleId, int viewCount)
        {
            //Arrange
            var services = CreateServiceProvider();
            var metrics = services.GetRequiredService<IApprenticeAppMetrics>();
            var meterFactory = services.GetRequiredService<IMeterFactory>();
            var collector = new MetricCollector<long>(meterFactory, "MyApprenticeApp", "MyApprentice-App.supportguidancearticle.views");

            // Act
            metrics.IncreaseSupportGuidanceArticleViews(articleId, viewCount);

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
            serviceCollection.AddSingleton<IApprenticeAppMetrics, ApprenticeAppMetrics>();
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
