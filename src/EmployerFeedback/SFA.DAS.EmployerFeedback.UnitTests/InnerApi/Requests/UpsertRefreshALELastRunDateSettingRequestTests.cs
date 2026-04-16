using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using System;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class UpsertRefreshALELastRunDateSettingRequestTests
    {
        [Test]
        public void Constructor_SetsDataProperty()
        {
            var now = DateTime.UtcNow;
            var request = new UpsertRefreshALELastRunDateSettingRequest(now);
            Assert.That(request.Data.Value, Is.EqualTo(now));
            Assert.That(request.PutUrl, Is.EqualTo("api/settings/RefreshALELastRunDate"));
        }
    }
}
