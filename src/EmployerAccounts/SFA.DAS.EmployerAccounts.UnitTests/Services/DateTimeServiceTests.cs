using System;
using System.Collections.Generic;
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
            ServiceCollection services
            )
        {
            var validDateTimeString = validTime.ToString();

            var inMemorySettings = new Dictionary<string, string> {
                {"EmployerAccountsOuterOverrideDatetime", validDateTimeString}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            
            // Act
            services.AddDateTimeServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var dateTimeService = serviceProvider.GetService<ICurrentDateTime>();

            dateTimeService.Should().NotBeNull();
            dateTimeService.Now.Should().Be(DateTime.Parse(validDateTimeString));
        }


    }
}