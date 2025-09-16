using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using System;
using System.Globalization;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class GetSettingsResponseTests
    {
        [Test]
        public void RefreshALELastRunDate_ReturnsDate_WhenPresentAndValid()
        {
            var date = DateTime.UtcNow;
            var response = new GetSettingsResponse
            {
                new GetSettingsItem { Name = SettingsKey.RefreshALELastRunDate.ToString(), Value = date.ToString("o", CultureInfo.InvariantCulture) }
            };
            Assert.That(response.RefreshALELastRunDate, Is.EqualTo(date.ToLocalTime()).Within(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void RefreshALELastRunDate_ReturnsNull_WhenNotPresent()
        {
            var response = new GetSettingsResponse();
            Assert.That(response.RefreshALELastRunDate, Is.Null);
        }

        [Test]
        public void RefreshALELastRunDate_ReturnsNull_WhenValueIsInvalid()
        {
            var response = new GetSettingsResponse
            {
                new GetSettingsItem { Name = SettingsKey.RefreshALELastRunDate.ToString(), Value = "not-a-date" }
            };
            Assert.That(response.RefreshALELastRunDate, Is.Null);
        }
    }
}
