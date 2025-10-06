using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class GetAccountsBatchRequestTests
    {
        [Test]
        public void Constructor_SetsBatchSizeCorrectly()
        {
            var batchSize = 10;
            var request = new GetAccountsBatchRequest(batchSize);

            Assert.That(request.BatchSize, Is.EqualTo(batchSize));
        }

        [Test]
        public void GetUrl_ReturnsExpectedValue_WithBatchSize()
        {
            var batchSize = 25;
            var request = new GetAccountsBatchRequest(batchSize);

            Assert.That(request.GetUrl, Is.EqualTo($"api/accounts?batchsize={batchSize}"));
        }
    }
}