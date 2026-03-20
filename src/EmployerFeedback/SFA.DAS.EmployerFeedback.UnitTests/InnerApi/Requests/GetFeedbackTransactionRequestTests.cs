using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class GetFeedbackTransactionRequestTests
    {
        [Test]
        public void Then_SetsCorrectUrl_WhenConstructed()
        {
            var feedbackTransactionId = 12345L;

            var request = new GetFeedbackTransactionRequest(feedbackTransactionId);

            Assert.That(request.GetUrl, Is.EqualTo("api/feedbacktransactions/12345"));
        }
    }
}