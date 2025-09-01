using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class GenerateFeedbackSummariesRequestTests
    {
        [Test]
        public void GetUrl_ReturnsExpectedValue()
        {
            var request = new GenerateFeedbackSummariesRequest();
            Assert.That(request.PostUrl, Is.EqualTo("api/dataload/generate-feedback-summaries"));
        }
    }
}
