using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class GetFeedbackTransactionsBatchRequestTests
    {
        [Test]
        public void Constructor_SetsProperties_WhenCalled()
        {
            var batchSize = 10;

            var request = new GetFeedbackTransactionsBatchRequest(batchSize);

            Assert.That(request.BatchSize, Is.EqualTo(batchSize));
        }

        [Test]
        public void GetUrl_ReturnsCorrectUrl_WhenBatchSizeIsProvided()
        {
            var batchSize = 25;
            var expectedUrl = "api/feedbacktransactions?batchsize=25";

            var request = new GetFeedbackTransactionsBatchRequest(batchSize);

            Assert.That(request.GetUrl, Is.EqualTo(expectedUrl));
        }
    }
}