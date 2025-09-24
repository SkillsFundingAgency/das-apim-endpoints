using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using System;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class GetSettingsResponseTests
    {
        [Test]
        public void Value_ReturnsDate_WhenPresentAndValid()
        {
            var date = DateTime.UtcNow;
            var response = new GetSettingsResponse
            {
                Value = date
            };
            Assert.That(response.Value, Is.EqualTo(date));
        }

        [Test]
        public void Value_ReturnsNull_WhenNotPresent()
        {
            var response = new GetSettingsResponse();
            Assert.That(response.Value, Is.Null);
        }
    }
}
