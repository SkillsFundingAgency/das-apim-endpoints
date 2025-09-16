using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class GetSettingsRequestTests
    {
        [Test]
        public void GetUrl_ReturnsExpectedValue()
        {
            var request = new GetSettingsRequest();
            Assert.That(request.GetUrl, Is.EqualTo("api/settings"));
        }
    }
}
