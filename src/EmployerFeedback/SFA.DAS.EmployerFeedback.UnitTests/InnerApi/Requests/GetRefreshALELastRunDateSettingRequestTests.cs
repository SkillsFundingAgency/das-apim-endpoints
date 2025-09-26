using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class GetRefreshALELastRunDateSettingRequestTests
    {
        [Test]
        public void GetUrl_ReturnsExpectedValue()
        {
            var request = new GetRefreshALELastRunDateSettingRequest();
            Assert.That(request.GetUrl, Is.EqualTo("api/settings/RefreshALELastRunDate"));
        }
    }
}
