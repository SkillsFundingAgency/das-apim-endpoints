using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Services
{
    [TestFixture]
    public class DateTimeServiceTests
    {
        [Test, MoqAutoData]
        public void AddDateTimeServices_ShouldAddCurrentDateTimeToServiceCollection(
            DateTime validTime,
            ServiceCollection services,
            [Frozen] Mock<IConfiguration> configuration
            )
        {
            var validDateTimeString = validTime.ToString();

            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("EmployerAccountsOuterOverrideDatetime"))])
                .Returns(validDateTimeString);

            // Act
            services.AddDateTimeServices(configuration.Object);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var dateTimeService = serviceProvider.GetService<ICurrentDateTime>();

            dateTimeService.Should().NotBeNull();
            dateTimeService.Now.Should().Equals(DateTime.Parse(validDateTimeString));
        }

        [Test, MoqAutoData]
        public void AddDateTimeServices_WithInvalidDateTimeString_ShouldUseCurrentDateTime(
            ServiceCollection services,
            [Frozen] Mock<IConfiguration> configuration
            )
        {
            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("EmployerAccountsOuterOverrideDatetime"))])
                .Returns("");

            services.AddDateTimeServices(configuration.Object);

            var serviceProvider = services.BuildServiceProvider();
            var dateTimeService = serviceProvider.GetService<ICurrentDateTime>();

            dateTimeService.Should().NotBeNull();
            dateTimeService.Now.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}