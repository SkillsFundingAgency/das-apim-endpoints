using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.CustomerEngagementFinanceServiceTests
{
    [TestFixture]
    public class WhenParsingCustomerExperienceJson
    {
        [Test]
        public void Then_the_VRF_status_updated_date_is_deserialized()
        {
            // Arrange
            var json = CustomerExperienceServiceResponses.VrfJson;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());

            // Act
            var responseBody = JsonSerializer.Deserialize<VendorRegistrationCase>(json, options);

            // Assert
            var lastUpdatedDate = DateTime.Parse(responseBody.CaseStatusLastUpdatedDate);

            lastUpdatedDate.Year.Should().Be(2020);
            lastUpdatedDate.Month.Should().Be(8);
            lastUpdatedDate.Day.Should().Be(19);
            lastUpdatedDate.Hour.Should().Be(8);
            lastUpdatedDate.Minute.Should().Be(54);
            lastUpdatedDate.Second.Should().Be(42);
        }
    }
}
